using Model;

namespace DAL.Interfaces
{
    public interface IWorkOrderRepository
    {
        IEnumerable<WorkOrder> GetAll();
        WorkOrder? GetById(int id);
        void Add(WorkOrder workOrder);
        void Update(WorkOrder workOrder);
        void Delete(int id);
    }
}