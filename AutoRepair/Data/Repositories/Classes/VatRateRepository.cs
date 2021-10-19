using AutoRepair.Data.Entities;
using AutoRepair.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Classes
{
    public class VatRateRepository : GenericRepository<VatRate>, IVatRateRepository
    {
        private readonly DataContext _context;

        public VatRateRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
