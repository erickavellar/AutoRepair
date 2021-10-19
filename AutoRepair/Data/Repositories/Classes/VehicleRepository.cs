using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Classes
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public VehicleRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }      
               

        public IQueryable GetAllWithUsers()
        {
            return _context.Vehicles.Include(p => p.User);
        }
        

        public IEnumerable<SelectListItem> GetComboVehicles()
        {
            var list = _context.Vehicles.Select(p => new SelectListItem
            {
                Text = p.LicencePlate,
                Value = p.Id.ToString()
            }).ToList();

            
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a vehicle...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboUser()
        {
            var list = _context.Users.Select(c => new SelectListItem
            {
                Text = c.FirstName,
                Value = c.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a user...)",
                Value = "0"
            });

            return list;
        }

        public async Task<Vehicle> GetByIdVehicleAsync(int id)
        {
            return await _context.Set<Vehicle>().AsNoTracking().Include(p => p.User).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Vehicle> GetUserVehicle(int id)
        {
            return await _context.Vehicles
                .Include(v => v.User)
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Vehicle> GetUserVehicles(string userId)
        {
            var result = _context.Vehicles.Include(b => b.Model).Include(b => b.Brand).Include(v => v.Category).Include(v => v.Color).Where(v => v.User.Id == userId);
            return result;

        }
    }
}

