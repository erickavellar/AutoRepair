using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserHelper _userHelper;
        private readonly IServiceRepository _serviceRepository;

        public ConverterHelper(IBrandRepository brandRepository, ICategoryRepository categoryRepository,
            IVehicleRepository vehicleRepository, IUserHelper userHelper, IServiceRepository serviceRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _vehicleRepository = vehicleRepository;
            _userHelper = userHelper;
            _serviceRepository = serviceRepository;
        }

        public Brand ToBrand(BrandViewModel model, bool isNew)
        {
            return new Brand
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                IsActive = true
            };
        }

        public BrandViewModel ToBrandViewModel(Brand model)
        {
            return new BrandViewModel
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = true
            };
        }

        public Category ToCategory(CategoryViewModel model, bool isNew)
        {
            return new Category
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                IsActive = true
            };
        }

        public CategoryViewModel ToCategoryViewModel(Category model)
        {
            return new CategoryViewModel
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = true
            };
        }

        

        
        public Service ToService(ServiceViewModel model, bool isNew)
        {
            return new Service
            {
                Id = isNew ? 0 : model.Id,
                Description = model.Description,
                CreationDate = model.CreationDate,
                UpdateDate = model.UpdateDate,
                DeactivationDate = model.DeactivationDate,
                CostPrice = model.CostPrice,
                SalePrice = model.SalePrice,
                IsActive = true
            };
        }

        public ServiceViewModel ToServiceViewModel(Service model)
        {
            return new ServiceViewModel
            {
                Id = model.Id,
                Description = model.Description,
                CreationDate = model.CreationDate,
                UpdateDate = model.UpdateDate,
                DeactivationDate = model.DeactivationDate,
                CostPrice = model.CostPrice,
                SalePrice = model.SalePrice,
                IsActive = true
            };
        }

        public User ToUser(RegisterNewUserViewModel model, string path, bool isNew)
        {
            return new User
            {
                
                PhotoUrl = path,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                isActive = true
            };
        }

        public RegisterNewUserViewModel ToRegisterNewUserViewModel(User model)
        {
            return new RegisterNewUserViewModel
            {
                imageUrl = model.PhotoUrl,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber                

            };
        }

        public Vehicle ToVehicle(VehicleViewModel model, bool isNew)
        {
            return new Vehicle
            {
                Id = isNew ? 0 : model.Id,
                User = model.User,
                CreationDate = model.CreationDate,
                UpdateDate = model.UpdateDate,
                DeactivationDate = model.DeactivationDate,
                LicencePlate = model.LicencePlate,
                Category = model.Category,
                Brand = model.Brand,
                Model = model.Model,
                Mileage = model.Mileage,
                Color = model.Color,
                IsActive = true
            };
        }

        public VehicleViewModel ToVehicleViewModel(Vehicle vehicle)
        {
            return new VehicleViewModel
            {
                Id = vehicle.Id,
                User = vehicle.User,
                CreationDate = vehicle.CreationDate,
                UpdateDate = vehicle.UpdateDate,
                DeactivationDate = vehicle.DeactivationDate,
                LicencePlate = vehicle.LicencePlate,
                Category = vehicle.Category,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Mileage = vehicle.Mileage,
                Color = vehicle.Color,
                IsActive = true
            };
        }

        public Repair ToRepair(RepairViewModel model, bool isNew)
        {
            return new Repair
            {
                Id = isNew ? 0 : model.Id,
                Schedule = model.Schedule,
                User = model.User,
                DesactivationDate = model.DesactivationDate
            };
        }

        public RepairViewModel ToRepairViewModel(Repair vehicle)
        {
            return new RepairViewModel
            {
                Id = vehicle.Id,
                User = vehicle.User,
                Schedule = vehicle.Schedule,
                DesactivationDate = vehicle.DesactivationDate
            };
        }
    }
}
