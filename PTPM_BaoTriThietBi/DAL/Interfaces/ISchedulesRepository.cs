using Model;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public partial interface ISchedulesRepository
    {
        bool Create(Schedules model);
        Schedules GetDatabyID(string id);
        List<Schedules> GetDataAll();
        bool Update(Schedules model);
        bool Delete(int scheduleId);
    }
}