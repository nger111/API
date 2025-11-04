using Model;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public partial interface IWorkOrdersBusiness
    {
        bool Create(WorkOrders model);
        WorkOrders GetDatabyID(string id);
        List<WorkOrders> GetDataAll();
        bool Update(WorkOrders model);
        bool Delete(int workOrderId);
    }
}