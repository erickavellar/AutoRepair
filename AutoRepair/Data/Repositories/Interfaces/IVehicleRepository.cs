using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Interfaces
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        public IQueryable GetAllWithUsers();
        
        IEnumerable<SelectListItem> GetComboUser();

        Task<Vehicle> GetByIdVehicleAsync(int id);

        IEnumerable<SelectListItem> GetComboVehicles();

        Task<Vehicle> GetUserVehicle(int id);

        IEnumerable<Vehicle> GetUserVehicles(string userID);

    }
}
