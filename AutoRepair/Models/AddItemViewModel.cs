using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class AddItemViewModel
    {

        [Display(Name = "Service")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a service.")]
        public int ServiceId { get; set; }


        [Display(Name = "Vehicle")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a vehicle.")]
        public int VehicleId { get; set; }


        public IEnumerable<SelectListItem> Services { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }

    }
}
