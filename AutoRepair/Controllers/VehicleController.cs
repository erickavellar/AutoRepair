using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    [Authorize(Roles = "Receptionist, Mechanic")]
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserHelper _userHelper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public VehicleController(IVehicleRepository vehicleRepository, IUserHelper userHelper, 
            ICategoryRepository categoryRepository, IBrandRepository brandRepository,
            IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _vehicleRepository = vehicleRepository;
            _userHelper = userHelper;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: VehicleController
        public IActionResult Index()
        {
            return View(_vehicleRepository.GetAll().OrderBy(p => p.Id));
        }

        // GET: VehicleController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);

            if (vehicle == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            return View(vehicle);
        }

        // GET: VehicleController/Create
        public IActionResult Create()
        {
            var model = new VehicleViewModel
            {
                Users = _vehicleRepository.GetComboUser()
            };
            
            return View(model);
        }

        // POST: VehicleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel models)
        {

            if (ModelState.IsValid)
            {
                var vehicle = _converterHelper.ToVehicle(models, true);
                vehicle.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _vehicleRepository.CreateAsync(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(models);
        }

        // GET: VehicleController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            
            var vehicle = await _vehicleRepository.GetByIdVehicleAsync(id);
            
            if (vehicle == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            var model = _converterHelper.ToVehicleViewModel(vehicle);
            model.Users = _vehicleRepository.GetComboUser();
            return View(model);
        }

        // POST: VehicleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleViewModel models)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var vehicle = _converterHelper.ToVehicle(models, false);
                    vehicle.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    await _vehicleRepository.UpdateAsync(models);                    
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _vehicleRepository.ExistAsync(models.Id))
                    {
                        return new NotFoundViewResult("VehicleNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(models);
        }

        // GET: VehicleController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);

            if (vehicle == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }
            await _vehicleRepository.DeleteAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
