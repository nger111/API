using Model;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public partial interface IPartUsagesRepository
    {
        bool Create(PartUsages model);
        PartUsages GetDatabyID(string id);
        List<PartUsages> GetDataAll();
        bool Update(PartUsages model);
        bool Delete(int partUsageId);
    }
}