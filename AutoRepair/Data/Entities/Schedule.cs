using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class Schedule : IEntity
    {
        public int Id { get; set; }


        [Required]
        public User User { get; set; }

        
                
        public string Service { get; set; }

        
        
        public string Vehicle { get; set; }

        [Required]
        [Display(Name = "Schedule date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime ScheduleDate { get; set; }


        [Display(Name = "Schedule date held")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? ScheduleDateHeld { get; set; }
  

        public IEnumerable<ScheduleDetail> Items { get; set; }


        [Display(Name = "Schedule date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? ScheduleDateLocal =>
#pragma warning disable CS8073 
            this.ScheduleDate == null
#pragma warning restore CS8073
            ? null
            : this.ScheduleDate.ToLocalTime();


    }
}
