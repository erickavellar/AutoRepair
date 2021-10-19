﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class ScheduleDetail : IEntity
    {
        public int Id { get; set; }


        [Required]
        public Service Service { get; set; }

        [Required]
        public Vehicle Vehicle { get; set; }

                
    }
}
