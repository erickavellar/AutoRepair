using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Data.Repositories.Interfaces
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<IQueryable<Schedule>> GetScheduleAsync(string userName);


        Task<IQueryable<ScheduleDetailTemp>> GetDetailTempsAsync(string userName);


        Task AddItemToScheduleAsync(AddItemViewModel model, string userName); 


        Task ModifyScheduleDetailTempQuantityAsync(int id);


        Task DeleteDetailTempAsync(int id);

        Task DeleteDetailAsync(int id);


        Task<bool> ConfirmScheduleAsync(string userName);


        Task<Schedule> GetScheduleAsync(int id);

        Task ScheduleHeld(ScheduleHeldViewModel model);
    }
}
