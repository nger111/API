using Model;

namespace DAL.Interfaces
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> GetAll();
        Schedule? GetById(int id);
        void Add(Schedule schedule);
        void Update(Schedule schedule);
        void Delete(int id);
    }
}