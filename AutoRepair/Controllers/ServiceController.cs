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
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IConverterHelper _converterHelper;

        public ServiceController(IServiceRepository serviceRepository, IConverterHelper converterHelper)
        {
            _serviceRepository = serviceRepository;
            _converterHelper = converterHelper;
        }


        // GET: ServiceController
        public IActionResult Index()
        {
            return View(_serviceRepository.GetAll().OrderBy(o => o.Description));
        }

        // GET: ServiceController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var service = await _serviceRepository.GetByIdAsync(id.Value);
                       
            if (service == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            return View(service);
        }

        // GET: ServiceController/Create
        public IActionResult Create()
        {            
            return View();
        }

        // POST: ServiceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                
                service.IsActive = true;
                await _serviceRepository.CreateAsync(service);
                return RedirectToAction(nameof(Index));
                  
            }
            return View(service);
        }

        // GET: ServiceController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var model = _converterHelper.ToServiceViewModel(service);
            return View(service);
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceRepository.UpdateAsync(model);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _serviceRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("ServiceNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(model);
        }

        // GET: ServiceController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var service = await _serviceRepository.GetByIdAsync(id.Value);
            
            if (service == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            return View(service);
        }

        // POST: ServiceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            try
            {
                await _serviceRepository.DeleteAsync(service);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
