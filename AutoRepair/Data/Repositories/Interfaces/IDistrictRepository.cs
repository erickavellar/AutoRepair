using AutoRepair.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories
{
    public interface IDistrictRepository : IGenericRepository<District>
    {

        IEnumerable<SelectListItem> GetComboDistrict();

       
    }
}
