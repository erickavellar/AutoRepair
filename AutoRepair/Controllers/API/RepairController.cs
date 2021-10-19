using AutoRepair.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RepairController : Controller
    {
        private readonly IRepairRepository _repairRepository;

        public RepairController(IRepairRepository repairRepository)
        {
            _repairRepository = repairRepository;
        }


        [HttpGet]
        public IActionResult GetRepair()
        {
            return Ok(_repairRepository.GetVehicle());
        }
    }
}
