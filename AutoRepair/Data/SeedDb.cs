using AutoRepair.Data.Entities;
using AutoRepair.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }


        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Mechanic");
            await _userHelper.CheckRoleAsync("Receptionist");
            await _userHelper.CheckRoleAsync("Customer");

            var user = await _userHelper.GetUserByEmailAsync("mechanicalworkshop092021@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Erick",
                    LastName = "Avellar",
                    Email = "mechanicalworkshop092021@gmail.com",
                    UserName = "mechanicalworkshop092021@gmail.com",
                    EmailConfirmed = true
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }


            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Districts.Any())
            {
                AddDistrict("Aveiro");
                AddDistrict("Beja");
                AddDistrict("Braga");
                AddDistrict("Bragança");
                AddDistrict("Castelo Branco");
                AddDistrict("Coimbra");
                AddDistrict("Évora");
                AddDistrict("Faro");
                AddDistrict("Funchal");
                AddDistrict("Guarda");
                AddDistrict("Horta");
                AddDistrict("Lamego");
                AddDistrict("Leiria");
                AddDistrict("Lisboa");
                AddDistrict("Portoalegre");
                AddDistrict("Porto");
                AddDistrict("Santarém");
                AddDistrict("Viana do Castelo");
                AddDistrict("Vila Real");
                AddDistrict("Viseu");

                await _context.SaveChangesAsync();
            }

            if (!_context.Categories.Any())
            {                
                AddCategory("Cabrio");
                AddCategory("Van");
                AddCategory("City");
                AddCategory("Coupe");
                AddCategory("Minivan");
                AddCategory("Small Town");
                AddCategory("Sedan");
                AddCategory("SUV");
                AddCategory("Utility");
                await _context.SaveChangesAsync();
            }


            if (!_context.Brands.Any())
            {
                AddBrand("BMW");
                AddBrand("Mercedes-Benz");
                AddBrand("Renault");
                AddBrand("Peugeot");
                AddBrand("Volvo");
                AddBrand("Nissan");
                AddBrand("Land Rover");
                AddBrand("Fiat");
                AddBrand("Citroen");
                AddBrand("Porche");
                AddBrand("Ford");
                AddBrand("Hummer");
                AddBrand("Jaguar");
                AddBrand("Chevrolet");
                AddBrand("Kia");
                AddBrand("Audio");
                AddBrand("Jeep");
                AddBrand("Volkswagen");

                await _context.SaveChangesAsync();
            }


            if (!_context.Services.Any())
            {
                AddService("Revision");
                AddService("Alignment");
                AddService("Polishing");
                AddService("Clutch");
                AddService("Tire Change");

                await _context.SaveChangesAsync();
            }


            if (!_context.VatRates.Any())
            {
                AddVatRate(0.23);

                await _context.SaveChangesAsync();
            }

        }

        private void AddVatRate(double value)
        {
            _context.VatRates.Add(new VatRate 
            {
                VATRate = value
            });
        }

        private void AddService(string name)
        {
            _context.Services.Add(new Service
            {
                Description = name,
                CreationDate = DateTime.Now,
                CostPrice = _random.Next(1000),
                SalePrice = _random.Next(1000),
                IsActive = true,
            });
        }

        private void AddBrand(string name)
        {
            _context.Brands.Add(new Brand
            {
                Name = name
            });
        }

        private void AddDistrict(string name)
        {
            _context.Districts.Add(new District
            {
                Name = name
            });
        }

        private void AddCategory(string name)
        {
            _context.Categories.Add(new Category
            {
                Name = name
            });
        }
    }
}
