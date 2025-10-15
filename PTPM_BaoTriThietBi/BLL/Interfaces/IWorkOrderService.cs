using Model;

namespace BLL.Interfaces
{
    public interface IWorkOrderService
    {
        IEnumerable<WorkOrder> GetAll();
        WorkOrder? GetById(int id);
        void Add(WorkOrder workOrder);
        void Update(WorkOrder workOrder);
        void Delete(int id);
    }
}