using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Service : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }


        [Required]
        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? CreationDate { get; set; }



        
        [Display(Name = "Update Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? UpdateDate { get; set; }



        
        [Display(Name = "Deactivation Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DeactivationDate { get; set; }


        [Required]
        [Display(Name = "Cost Price")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal CostPrice { get; set; }


        [Required]
        [Display(Name = "Sale Price")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal SalePrice { get; set; }


        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

    }
}
