using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories;
using AutoRepair.Data.Repositories.Interfaces;
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
    public class MechanicController : Controller
    {
        
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IDistrictRepository _districtRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public MechanicController(IUserHelper userHelper, 
            IMailHelper mailHelper, IImageHelper imageHelper, 
            IConverterHelper converterHelper, IDistrictRepository districtRepository,
            UserManager<User> userManager, IConfiguration configuration)
        {
            
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _districtRepository = districtRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: MechanicController
        public async Task<IActionResult> Index()
        {
            var client = await _userManager.GetUsersInRoleAsync("Mechanic");
            var userRolesViewModel = new List<UserRoleViewModel>();
            foreach (User user in client)
            {
                var model = new UserRoleViewModel();
                model.Id = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;
                model.Email = user.Email;
                model.isActive = user.isActive;
                userRolesViewModel.Add(model);
            }
            return View(userRolesViewModel);
        }

        
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            var mechanic = await _userHelper.GetUserByIdAsync(id);



            if (mechanic == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            var model = new RegisterNewUserViewModel();
            if (mechanic != null)
            {
                model.imageUrl = mechanic.PhotoUrl;
                model.FirstName = mechanic.FirstName;
                model.LastName = mechanic.LastName;
                model.Address = mechanic.Address;
                model.PhoneNumber = mechanic.PhoneNumber;
                model.imageUrl = mechanic.PhotoUrl;
                model.Username = mechanic.Email;
            }

            return View(mechanic);
        }

        // GET: MechanicController/Create
        public IActionResult Create()
        {
           
            var model = new RegisterNewUserViewModel
            {

                Districts = _districtRepository.GetComboDistrict(),
            };
            return View(model);
        }

        // POST: MechanicController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterNewUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;
                    if (model.PhotoUrl != null)
                    {
                        path = await _imageHelper.UploadImageAsync(model.PhotoUrl, "Mechanic");
                    }

                    var user = await _userHelper.GetUserByEmailAsync(model.Username);

                    if (user == null)
                    {
                        user = new User
                        {
                            PhotoUrl = model.imageUrl,
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

                        await _userHelper.AddUserToRoleAsync(user, "Mechanic");

                        string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        string tokenLink = Url.Action("ConfirmEmail", "Mechanic", new
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
                            return RedirectToAction("Index", "Dashboard");
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

            }

            return View();
        }

        // GET: MechanicController/Edit/5
        public async Task<IActionResult> ChangeUser()
        {
            var mechanic = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (mechanic != null)
            {
                
                model.FirstName = mechanic.FirstName;
                model.LastName = mechanic.LastName;
                model.Address = mechanic.Address;                
                model.PhoneNumber = mechanic.PhoneNumber;
                
            }

            
            model.Districts = _districtRepository.GetComboDistrict();
            return View(model);
        }

        // POST: MechanicController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var mechanic = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    if (mechanic != null)
                    {
                        mechanic.FirstName = model.FirstName;
                        mechanic.LastName = model.LastName;
                        mechanic.Address = model.Address;
                        mechanic.PhoneNumber = model.PhoneNumber;

                        var response = await _userHelper.UpdateUserAsync(mechanic);
                        if (response.Succeeded)
                        {
                            ViewBag.UserMessage = "User updated!";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return new NotFoundViewResult("MechanicNotFound");
                }
            }
            return View(model);
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
                    var mechanic = await _userHelper.GetUserByIdAsync(model.UserId);

                    if (mechanic == null)
                    {
                        mechanic = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    }

                    if (mechanic != null)
                    {
                        var result = await _userHelper.ChangePasswordAsync(mechanic, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            if (Request.Query.Keys.Contains("ReturnUrl"))
                            {
                                return Redirect(Request.Query["ReturnUrl"].First());
                            }

                            return RedirectToAction("ChangeUser");
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
                    var mechanic = await _userHelper.GetUserByEmailAsync(model.Email);
                    if (mechanic == null)
                    {
                        ModelState.AddModelError(string.Empty, "The email doesn't correspond to a registered mechanic.");
                        return this.View(model);
                    }

                    var myToken = await _userHelper.GeneratePasswordResetTokenAsync(mechanic);

                    var link = this.Url.Action(
                        "ResetPassword",
                        "Accounts",
                        new { token = myToken }, protocol: HttpContext.Request.Scheme);

                    _mailHelper.SendMail(model.Email, "Auto Repair Password Reset", $"<h1>Auto Repair Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                    return this.View();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return this.View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            var mechanic = await _userManager.FindByIdAsync(id);

            if (mechanic == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            await _userManager.DeleteAsync(mechanic);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
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

                        return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
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


        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
