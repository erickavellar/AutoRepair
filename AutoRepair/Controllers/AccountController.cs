using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IDistrictRepository _districtRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(IUserHelper userHelper, IConfiguration configuration, 
            IMailHelper mailHelper, IDistrictRepository districtRepository, IImageHelper imageHelper,
             IConverterHelper converterHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _districtRepository = districtRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// Saves the Login data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userHelper.LoginAsync(model);

                    if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First());
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            this.ModelState.AddModelError(string.Empty, "Failed to Login.");
            return View(model);
        }

        /// <summary>
        /// Returns the Logout View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogOutAsync();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns the Register New User View
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {

                Districts = _districtRepository.GetComboDistrict(),
            };
            return View(model);
        }

        /// <summary>
        /// Saves the Register New User data
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Register</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;
                    if (model.PhotoUrl != null && model.PhotoUrl.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.PhotoUrl, "users");
                    }

                    var user = await _userHelper.GetUserByEmailAsync(model.Username);

                    if (user == null)
                    {
                        user = new User
                        {
                            PhotoUrl = path,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Address = model.Address,
                            PhoneNumber = model.PhoneNumber,
                            UserName = model.Username,
                            Email = model.Username
                        };

                        var result = await _userHelper.AddUserAsync(user, model.Password);

                        if (result != IdentityResult.Success)
                        {
                            ModelState.AddModelError(string.Empty, "The user couldn´t be created");
                            return View(model);
                        }

                        await _userHelper.AddUserToRoleAsync(user, "Customer");

                        string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        string tokenLink = Url.Action("ConfirmEmail", "Account", new
                        {
                            userid = user.Id,
                            token = myToken
                        }, protocol: HttpContext.Request.Scheme);

                        Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                            $"To allow the user, " +
                            $"please click in this link:</br></br><a href = \"{ tokenLink}\">Confirm Email</a>");


                        if (response.IsSuccess)
                        {
                            ViewBag.Message = "The instructions to allow you user has been sent to email";
                            return View(model);
                        }

                        var loginViewModel = new LoginViewModel
                        {
                            Password = model.Password,
                            RememberMe = false,
                            Username = model.Username
                        };

                        var result2 = await _userHelper.LoginAsync(loginViewModel);
                        if (result2.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        this.ModelState.AddModelError(string.Empty, "The user already exists.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(model);
        }

        /// <summary>
        /// Returns the View after E-mail confirmation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns>View</returns>
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        /// <summary>
        /// Returns the Change User View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                //model.PhotoUrl = user.PhotoUrl;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;
            }

            return View(model);
        }

        /// <summary>
        /// Change Properties of the User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>View</returns>
        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "users");
                    }

                    var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        model.Address = user.Address;

                        var response = await _userHelper.UpdateUserAsync(user);
                        if (response.Succeeded)
                        {
                            ViewBag.UserMessage = "User updated!";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return this.View(model);
        }

        /// <summary>
        /// Returns the Change Password View
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Saves the Change Password data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByIdAsync(model.UserId);

                    if (user == null)
                    {
                        user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    }

                    if (user != null)
                    {
                        var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            if (Request.Query.Keys.Contains("ReturnUrl"))
                            {
                                return Redirect(Request.Query["ReturnUrl"].First());
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                        }
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, "User Not Found!");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(model);
        }

        /// <summary>
        /// Returns the Recover Password View
        /// </summary>
        /// <returns></returns>
        public IActionResult RecoverPassword()
        {
            return View();
        }

        /// <summary>
        /// Saves the Recover Password data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "The email doesn't correspond to a registered user.");
                        return this.View(model);
                    }

                    //var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                    //var link = this.Url.Action(
                    //    "ResetPassword",
                    //    "Accounts",
                    //    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                    //_mailHelper.SendMail(model.Email, "School Password Reset", $"<h1>School Password Reset</h1>" +
                    //$"To reset the password click in this link:</br></br>" +
                    //$"<a href = \"{link}\">Reset Password</a>");
                    //this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                    //return this.View();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return this.View(model);
        }

        /// <summary>
        /// Returns the Reset Password View
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        /// <summary>
        /// Saves the Reset Password data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);
                if (user != null)
                {
                    var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        this.ViewBag.Message = "Password reset successful.";
                        return this.View();
                    }

                    this.ViewBag.Message = "Error while resetting the password.";
                    return View(model);
                }

                this.ViewBag.Message = "User not found.";
                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Returns the Not Authorized View
        /// </summary>
        /// <returns></returns>
        public IActionResult NotAuthorized()
        {
            return View();
        }

        
    }
}
