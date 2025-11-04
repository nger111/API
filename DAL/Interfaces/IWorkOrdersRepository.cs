using Model;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public partial interface IWorkOrdersRepository
    {
        bool Create(WorkOrders model);
        WorkOrders GetDatabyID(string id);
        List<WorkOrders> GetDataAll();
        bool Update(WorkOrders model);
        bool Delete(int workOrderId);
    }
}