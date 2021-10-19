using AutoRepair.Data.Entities;
using AutoRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public interface IConverterHelper
    {
        User ToUser(RegisterNewUserViewModel model, string path, bool isNew);
        RegisterNewUserViewModel ToRegisterNewUserViewModel(User model);

        Vehicle ToVehicle(VehicleViewModel model, bool isNew);
        VehicleViewModel ToVehicleViewModel(Vehicle vehicle);

        Category ToCategory(CategoryViewModel model, bool isNew);
        CategoryViewModel ToCategoryViewModel(Category model);

        Brand ToBrand(BrandViewModel model, bool isNew);
        BrandViewModel ToBrandViewModel(Brand model);

        Service ToService(ServiceViewModel model, bool isNew);
        ServiceViewModel ToServiceViewModel(Service model);

        Repair ToRepair(RepairViewModel model, bool isNew);
        RepairViewModel ToRepairViewModel(Repair vehicle);

    }
}
