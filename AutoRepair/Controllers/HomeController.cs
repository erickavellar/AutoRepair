using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public HomeController(ILogger<HomeController> logger, IVehicleRepository vehicleRepository, 
            IScheduleRepository scheduleRepository)
        {
            _logger = logger;
            _vehicleRepository = vehicleRepository;
            _scheduleRepository = scheduleRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Vehicle()
        {
            return View(_vehicleRepository.GetAll().OrderBy(p => p.Id));
        }

        public async Task<IActionResult> Schedule()
        {
            var model = await _scheduleRepository.GetScheduleAsync(this.User.Identity.Name);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
