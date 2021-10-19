using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class RepairViewModel : Repair
    {
        public IEnumerable<SelectListItem> Schedules { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }

    
}
