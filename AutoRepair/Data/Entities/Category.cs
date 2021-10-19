using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
