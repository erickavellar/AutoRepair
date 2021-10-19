using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    [Authorize(Roles = "Admin, Mechanic, Receptionist")]
    public class DashboardController : Controller
    {        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
