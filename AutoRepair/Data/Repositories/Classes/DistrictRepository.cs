using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories
{
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        private readonly DataContext _context;

        public DistrictRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        
        public IEnumerable<SelectListItem> GetComboDistrict()
        {
            var list = _context.Districts.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a city...)",
                Value = "0"
            });

            return list;
        }
        
    }
}
