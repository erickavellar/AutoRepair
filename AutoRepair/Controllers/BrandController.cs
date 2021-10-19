using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;

        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        // GET: BrandController
        public IActionResult Index()
        {
            return View(_brandRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: BrandController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BrandNotFound");
            }

            var brand = await _brandRepository.GetByIdAsync(id.Value);
            if (brand == null)
            {
                return new NotFoundViewResult("BrandNotFound");
            }

            return View(brand);
        }

        // GET: BrandController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    await _brandRepository
                        .CreateAsync(brand);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    return View(brand);
                    
                }

            }
            return View(brand);
        }

        // GET: BrandController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BrandNotFound");
            }

            var brand = await _brandRepository
                .GetByIdAsync(id.Value);
            if (brand == null)
            {
                return new NotFoundViewResult("BrandNotFound");
            }
            return View(brand);
        }

        // POST: BrandController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {  
                    await _brandRepository.UpdateAsync(brand);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _brandRepository.ExistAsync(brand.Id))
                    {
                        return new NotFoundViewResult("BrandNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: BrandController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BrandNotFound");
            }

            var brand = await _brandRepository
                .GetByIdAsync(id.Value);
            if (brand == null)
            {
                return new NotFoundViewResult("BrandNotFound");
            }

            return View(brand);
        }

        // POST: BrandController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepository
                .GetByIdAsync(id);
            try
            {

                await _brandRepository
                    .DeleteAsync(brand);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (ex.InnerException.Message.Contains("REFERENCE constraint"))
                {

                    if (ModelState.IsValid)
                    {
                        
                        ViewBag.Error = $"There are Vehicles associated with {brand.Name}, so it can not be deleted, please deactivate";
                        return View(brand);
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    return View(brand);
                }
            }

            return View(brand);
        }
    }
}
