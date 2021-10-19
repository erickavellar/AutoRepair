using AutoRepair.Data;
using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;

        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager, DataContext context, IImageHelper imageHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _imageHelper = imageHelper;
        }


        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public IQueryable<User> GetAllWithUsers()
        {
            return _context.Users
                .Where(c => c.isActive)
                .Include(c => c.FullName);
        }

        public async Task<string> GetClientStatusRoleAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        public IEnumerable<SelectListItem> GetComboUsers()
        {
            var list = _context.Users.Select(p => new SelectListItem  
            {
                Text = p.FullName,
                Value = p.Id.ToString()
            }).ToList();

            
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a customer...)",
                Value = "0"
            });

            return list;
        }

        public async Task<User> GetEmployeeIdAsync(string id)
        {
            return await _userManager.Users
                .Include(c => c.Id)
                .Include(c => c.FirstName)
                .Include(c => c.LastName)
                .Include(c => c.PhotoUrl)
                .Include(c => c.PhoneNumber)
                .Include(c => c.Email)
                .Include(c => c.Address)
                .Where(c => c.isActive == true)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task RemoveFromRoleAsync(User user, string role)
        {
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public async Task<User> GetFullEmployeeByUserAsync(string userId)
        {
            return await _context.Users
                .Include(e => e.FullName)
                .Where(e => e.Id == userId)
                .FirstOrDefaultAsync();
        }
    }
}
