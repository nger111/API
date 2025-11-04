using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class WorkOrdersBusiness : IWorkOrdersBusiness
    {
        private readonly IWorkOrdersRepository _repo;

        public WorkOrdersBusiness(IWorkOrdersRepository repo)
        {
            _repo = repo;
        }

        public bool Create(WorkOrders model) => _repo.Create(model);
        public WorkOrders GetDatabyID(string id) => _repo.GetDatabyID(id);
        public List<WorkOrders> GetDataAll() => _repo.GetDataAll();
        public bool Update(WorkOrders model) => _repo.Update(model);
        public bool Delete(int workOrderId) => _repo.Delete(workOrderId);
    }
}