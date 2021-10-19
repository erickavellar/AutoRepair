using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<List<Service>> GetAllServicesAsync();

        IEnumerable<Service> GetAllServices();

        IEnumerable<SelectListItem> GetComboServices();

        Task<Service> GetServicesPerDayAsync(int servicesSuppliedId);

        IEnumerable<Service> GetWithServicesById(int id);

    }
}
