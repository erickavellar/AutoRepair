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
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IServiceRepository _serviceRepository;

        public ScheduleRepository(DataContext context, IUserHelper userHelper, IVehicleRepository vehicleRepository, 
            IServiceRepository serviceRepository) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _vehicleRepository = vehicleRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task AddItemToScheduleAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var service = await _context.Services.FindAsync(model.ServiceId);
            if (service == null)
            {
                return;
            }

            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null)
            {
                return;
            }

            var scheduleDetailTemp = await _context.ScheduleDetailsTemp
                .Where(sdt => sdt.User == user && sdt.Service == service && sdt.Vehicle == vehicle)
                .FirstOrDefaultAsync();

            if (scheduleDetailTemp == null)
            {
                scheduleDetailTemp = new ScheduleDetailTemp
                {  
                    Service = service,
                    Vehicle = vehicle,
                    User = user,
                };

                _context.ScheduleDetailsTemp.Add(scheduleDetailTemp);
            }
            else 
            {                
                _context.ScheduleDetailsTemp.Update(scheduleDetailTemp);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmScheduleAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }
            
            var scheduleTmps = await _context.ScheduleDetailsTemp
                .Include(o => o.Service)
                .Include(o => o.Vehicle)
                .Where(o => o.User == user)
                .ToListAsync();

            if (scheduleTmps == null || scheduleTmps.Count == 0)
            {
                return false;
            }
            
            var details = scheduleTmps.Select(s => new ScheduleDetail
            {
                Service = s.Service,
                Vehicle = s.Vehicle

            }).ToList();

            
            //aqui eu crio a schedule. tenho de passar o veiculo e servico
            var schedule = new Schedule
            {
                ScheduleDate = DateTime.UtcNow,
                User = user,
                Items = details,
                Service = details[0].Service.Description,
                Vehicle = details[0].Vehicle.LicencePlate
            };

            await CreateAsync(schedule);
            _context.ScheduleDetailsTemp.RemoveRange(scheduleTmps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var scheduleDetailTemp = await _context.ScheduleDetailsTemp.FindAsync(id);
            if (scheduleDetailTemp == null)
            {
                return;
            }

            _context.ScheduleDetailsTemp.Remove(scheduleDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDetailAsync(int id)
        {
            var scheduleDetail = await _context.ScheduleDetails.FindAsync(id);
            if (scheduleDetail == null)
            {
                return;
            }

            _context.ScheduleDetails.Remove(scheduleDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<ScheduleDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return _context.ScheduleDetailsTemp
                .Include(s => s.Service)
                .Include(s => s.Vehicle)
                .Where(s => s.User == user)
                .OrderBy(s => s.Service);
        }

        public async Task<Schedule> GetScheduleAsync(int id)
        {
            return await _context.Schedules.FindAsync(id);
        }

        public async Task<IQueryable<Schedule>> GetScheduleAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin, Mechanic, Receptionist"))
            {
                
                return _context.Schedules
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(s => s.Service)
                    //.Include(s => s. Vehicle)
                    .OrderBy(o => o.ScheduleDateHeld);
            }

            return _context.Schedules
                .Include(o => o.Items)
                .ThenInclude(s => s.Service)
                //.Include(p => p.Vehicle)
                .Where(o => o.User == user)
                .OrderBy(o => o.ScheduleDateHeld);
        }

        public async Task ModifyScheduleDetailTempQuantityAsync(int id)
        {
            var scheduleDetailTemp = await _context.ScheduleDetailsTemp.FindAsync(id);
            if (scheduleDetailTemp == null)
            {
                return;
            }
            _context.ScheduleDetailsTemp.Update(scheduleDetailTemp);
            await _context.SaveChangesAsync();
            
        }

        public async Task ScheduleHeld(ScheduleHeldViewModel model)
        {            
            var schedule = await _context.Schedules.FindAsync(model.Id);
            if (schedule == null)
            {
                return;
            }
            
            schedule.ScheduleDateHeld = model.ScheduleDateHeld;
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
        }
    }
}
