using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    [Authorize(Roles = "Receptionist, Mechanic")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMailHelper _mailHelper;
        private readonly IUserHelper _userHelper;

        public ScheduleController(IScheduleRepository scheduleRepository, IServiceRepository serviceRepository,
             IVehicleRepository vehicleRepository, IMailHelper mailHelper, IUserHelper userHelper)
        {
            _scheduleRepository = scheduleRepository;
            _serviceRepository = serviceRepository;
            _vehicleRepository = vehicleRepository;
            _mailHelper = mailHelper;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _scheduleRepository.GetScheduleAsync(this.User.Identity.Name);
            return View(model);
        }

        
        public async Task<IActionResult> Create()
        {
            var model = await _scheduleRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddVehicleAndService()
        {
            var model = new AddItemViewModel
            {
                Vehicles = _vehicleRepository.GetComboVehicles(),
                Services = _serviceRepository.GetComboServices()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicleAndService(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _scheduleRepository.AddItemToScheduleAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            await _scheduleRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ScheduleNotFound");
            }


            var schedule = await _scheduleRepository.GetByIdAsync(id.Value);

            if (schedule == null)
            {
                return new NotFoundViewResult("ScheduleNotFound");
            }
            await _scheduleRepository.DeleteDetailAsync(id.Value);
            await _scheduleRepository.DeleteAsync(schedule);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ScheduleNotFound");
            }

            await _scheduleRepository.ModifyScheduleDetailTempQuantityAsync(id.Value);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmSchedule()
        {
            var response = await _scheduleRepository.ConfirmScheduleAsync(this.User.Identity.Name);
            if (response)
            {
                return RedirectToAction("Index");
            }
            

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ScheduleHeld(int? id)
        {
            

            if (id == null)
            {
                return new NotFoundViewResult("ScheduleNotFound");
            }
            
            var schedule = await _scheduleRepository.GetScheduleAsync(id.Value);
            if (schedule == null)
            {
                return new NotFoundViewResult("ScheduleNotFound");
            }
            
            var model = new ScheduleHeldViewModel
            {
                Id = schedule.Id,
                ScheduleDateHeld = DateTime.Today,
                
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleHeld(ScheduleHeldViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _scheduleRepository.ScheduleHeld(model);
                return RedirectToAction(nameof(Index));
            }

            //Response response = _mailHelper.SendMail(model.User.Email, "Appointment Reschedule Confirmation",
            //             $"Dear Customer, " +
            //              $"Your vehicle has been scheduled for the day {model.ScheduleDateHeld}. <br /><br/>" +
            //              $"Thank you for your preference." + "<br/><br/> Auto Repair");

            return View();
        }
                
    }
}
