using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Entities
{
    public class District : IEntity
    {
        public int Id { get; set; }


        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} characters")]
        public string Name { get; set; }
    }
}
