using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Classes
{
    public class RepairRepository : GenericRepository<Repair>, IRepairRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly UserManager<User> _userManager;

        public RepairRepository(DataContext context, IUserHelper userHelper, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _userManager = userManager;
        }

        public Task AddRepairAsync(RepairViewModel model)
        {
            throw new NotImplementedException();
        }

        public IQueryable GetAllWithSchedules()
        {
            return _context.Repairs.Include(p => p.Id);
        }

        public IQueryable GetAllWithSchedulesName()
        {
            return _context.Repairs.Include(p => p.Schedule);
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Repairs.Include(p => p.User);
        }

        public async Task<Repair> GetByIdRepairAsync(int id)
        {
            return await _context.Set<Repair>().AsNoTracking().Include(p => p.User)
                .Include(p => p.Schedule)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public IEnumerable<SelectListItem> GetComboMechanic()
        {
            var list = _context.Users.Select(p => new SelectListItem
            {
                Text = p.FullName.ToString(),
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {

                Text = "(Select one Employee....)",
                Value = "0"

            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboRepair()
        {
            var list = _context.Repairs.Select(p => new SelectListItem
            {
                Text = p.Schedule.ToString(),
                Value = p.Id.ToString()
            }).ToList();

            
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a repair...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSchedule()
        {
            var list = _context.Schedules.Select(p => new SelectListItem
            {

                Text = $"{p.Vehicle}" + " - " + $" {p.Service}" + " - " + $" {p.ScheduleDateHeld}",
                Value = p.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {

                Text = "(Select one Schedule....)",
                Value = "0"

            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboServices()
        {
            var list = _context.Services.Select(p => new SelectListItem
            {

                Text = p.SalePrice.ToString(),
                Value = p.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {

                Text = "(Select one Service....)",
                Value = "0"

            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboVehicle()
        {
            var list = _context.Vehicles.Select(p => new SelectListItem
            {

                Text = p.LicencePlate,
                Value = p.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {

                Text = "(Select one Vehicle....)",
                Value = "0"

            });

            return list;
        }

        public IQueryable GetVehicle()
        {
            return _context.Repairs.Include(s => s.Schedule);
        }

        
    }
}
