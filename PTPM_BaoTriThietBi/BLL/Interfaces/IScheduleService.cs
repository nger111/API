using Model;

namespace BLL.Interfaces
{
    public interface IScheduleService
    {
        IEnumerable<Schedule> GetAll();
        Schedule? GetById(int id);
        void Add(Schedule schedule);
        void Update(Schedule schedule);
        void Delete(int id);
    }
}