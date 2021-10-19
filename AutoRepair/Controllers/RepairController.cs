using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    public class RepairController : Controller
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IScheduleRepository _scheduleRepository;

        public RepairController(IRepairRepository repairRepository, IScheduleRepository scheduleRepository, 
            IVehicleRepository vehicleRepository, IServiceRepository serviceRepository, IConverterHelper converterHelper,
             IUserHelper userHelper)
        {
            _repairRepository = repairRepository;
            _vehicleRepository = vehicleRepository;
            _serviceRepository = serviceRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _scheduleRepository = scheduleRepository;
        }

        // GET: RepairController
        public IActionResult Index()
        {
            return View(_repairRepository.GetAllWithSchedulesName());
        }

        // GET: RepairController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("RepairNotFound");
            }

            var repair = await _repairRepository.GetByIdAsync(id.Value);

            if (repair == null)
            {
                return new NotFoundViewResult("RepairNotFound");
            }

            return View(repair);
        }

        // GET: RepairController/Create
        public IActionResult Create()
        {
            var model = new RepairViewModel
            {
                
                Schedules = _repairRepository.GetComboSchedule(),
                Users = _repairRepository.GetComboMechanic()                
            };

            return View(model);
        }

        // POST: RepairController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    model.DesactivationDate = DateTime.Now;
                    await _repairRepository.CreateAsync(model);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    
                }

                return View(model);
            }

            return View(model);
        }

        // GET: RepairController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var repair = await _repairRepository.GetByIdRepairAsync(id);

            if (repair == null)
            {
                return new NotFoundViewResult("RepairNotFound");
            }

            var model = _converterHelper.ToRepairViewModel(repair);
            model.Users = _repairRepository.GetComboMechanic();
            model.Schedules = _repairRepository.GetComboSchedule();

            return View();
        }

        // POST: RepairController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RepairViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var repair = _converterHelper.ToRepair(model, false);
                    //model.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    await _repairRepository.UpdateAsync(model);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repairRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("RepairNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("RepairNotFound");
            }

            var repair = await _repairRepository.GetByIdAsync(id.Value);

            if (repair == null)
            {
                return new NotFoundViewResult("RepairNotFound");
            }
            await _repairRepository.DeleteAsync(repair);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
