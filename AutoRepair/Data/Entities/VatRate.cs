using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class VatRate : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "VAT Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double VATRate { get; set; }
    }
}
