using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Classes
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly DataContext _context;

        public ServiceRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboServices()
        {
            var list = _context.Services.Select(p => new SelectListItem
            {
                Text = p.Description,
                Value = p.Id.ToString()
            }).ToList();

            
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a service...)",
                Value = "0"
            });

            return list;
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _context.Services.Where(s => s.IsActive == true).ToListAsync();
        }

        public IEnumerable<Service> GetAllServices()
        {
            return _context.Services.Where(s => s.IsActive == true);
        }

        public async Task<Service> GetServicesPerDayAsync(int servicesSuppliedId)
        {
            return await _context.Services
                .FirstOrDefaultAsync(s => s.Id == servicesSuppliedId);
        }

        public IEnumerable<Service> GetWithServicesById(int id)
        {
            return _context.Services.Include(s => s.Id).Where(d => d.Id == id && d.IsActive == true).ToList();
        }
    }
}
