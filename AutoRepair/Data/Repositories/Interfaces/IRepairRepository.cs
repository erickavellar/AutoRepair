using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Interfaces
{
    public interface IRepairRepository : IGenericRepository<Repair>
    {
        public IQueryable GetAllWithUsers();

        public IQueryable GetAllWithSchedules();

        IEnumerable<SelectListItem> GetComboServices();

        IEnumerable<SelectListItem> GetComboSchedule();

        IEnumerable<SelectListItem> GetComboMechanic();

        IEnumerable<SelectListItem> GetComboVehicle();

        IEnumerable<SelectListItem> GetComboRepair();

        Task<Repair> GetByIdRepairAsync(int id);

        IQueryable GetAllWithSchedulesName();

        Task AddRepairAsync(RepairViewModel model);

        IQueryable GetVehicle();
    }
}
