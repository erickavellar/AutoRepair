using AutoRepair.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class CategoryViewModel : Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
