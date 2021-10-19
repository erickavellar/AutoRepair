using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Vehicle : IEntity
    {
        public int Id { get; set; }

        

        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }


        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }


        [Display(Name = "Deactivation Date")]
        public DateTime? DeactivationDate { get; set; }


        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }


        [Required]
        [Display(Name = "Licence PLate")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters long")]
        public string LicencePlate { get; set; }



        [Required]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters long")]
        public string Category { get; set; }



        [Required]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters long")]
        public string Brand { get; set; }        


        [Required]
        [MaxLength(30, ErrorMessage = "The field {0} only can contain {1} characters long")]
        public string Model { get; set; }

        [Required]
        public double Mileage { get; set; }


        [Required]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters long")]
        public string Color { get; set; }

        //[Required]
        [Display(Name = "User")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a User")]
        public string UserId { get; set; }

        

        public User User { get; set; }

        
    }
}
