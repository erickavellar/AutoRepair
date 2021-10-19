using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class VehicleViewModel : Vehicle
    {
        public IEnumerable<SelectListItem> Users { get; set; }
        
    }
}
