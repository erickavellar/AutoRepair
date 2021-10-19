using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class ChangeUserViewModel
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Address { get; set; }

        [Display(Name = "District")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        public int DistrictId { get; set; }


        public IEnumerable<SelectListItem> Districts { get; set; }

        [Display(Name = "Tax Number")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string TaxNumber { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string PhoneNumber { get; set; }
    }
}
