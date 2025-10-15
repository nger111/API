using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderRepository _repository;

        public WorkOrderService(IWorkOrderRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<WorkOrder> GetAll() => _repository.GetAll();

        public WorkOrder? GetById(int id) => _repository.GetById(id);

        public void Add(WorkOrder workOrder) => _repository.Add(workOrder);

        public void Update(WorkOrder workOrder) => _repository.Update(workOrder);

        public void Delete(int id) => _repository.Delete(id);
    }
}