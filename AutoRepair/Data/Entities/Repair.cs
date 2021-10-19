using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Repair : IEntity
    {
        public int Id { get; set; }


        public int ScheduleId { get; set; }


        public Schedule Schedule { get; set; }

        
        [Display(Name = "Deactivation Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DesactivationDate { get; set; }        


        public string MechanicId { get; set; }

        public User User { get; set; }
    }
}
