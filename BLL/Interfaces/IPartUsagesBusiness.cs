using Model;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public partial interface IPartUsagesBusiness
    {
        bool Create(PartUsages model);
        PartUsages GetDatabyID(string id);
        List<PartUsages> GetDataAll();
        bool Update(PartUsages model);
        bool Delete(int partUsageId);
    }
}