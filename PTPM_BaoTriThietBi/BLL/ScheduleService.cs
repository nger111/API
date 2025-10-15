using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _repository;

        public ScheduleService(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Schedule> GetAll() => _repository.GetAll();

        public Schedule? GetById(int id) => _repository.GetById(id);

        public void Add(Schedule schedule) => _repository.Add(schedule);

        public void Update(Schedule schedule) => _repository.Update(schedule);

        public void Delete(int id) => _repository.Delete(id);
    }
}