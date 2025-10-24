using Model;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public partial interface ISchedulesBusiness
    {
        bool Create(Schedules model);
        Schedules GetDatabyID(string id);
        List<Schedules> GetDataAll();
        bool Update(Schedules model);
        bool Delete(int scheduleId);
    }
}