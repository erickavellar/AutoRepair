using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Photo")]
        public string PhotoUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Address")]
        [StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Address { get; set; }

        
        public bool isActive { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(PhotoUrl))
                {
                    return null;
                }

                return $"https://localhost:44317{PhotoUrl.Substring(1)}";
            }
        }
    }
}
