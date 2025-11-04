using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class SchedulesBusiness : ISchedulesBusiness
    {
        private readonly ISchedulesRepository _repo;

        public SchedulesBusiness(ISchedulesRepository repo)
        {
            _repo = repo;
        }

        public bool Create(Schedules model) => _repo.Create(model);
        public Schedules GetDatabyID(string id) => _repo.GetDatabyID(id);
        public List<Schedules> GetDataAll() => _repo.GetDataAll();
        public bool Update(Schedules model) => _repo.Update(model);
        public bool Delete(int scheduleId) => _repo.Delete(scheduleId);
    }
}