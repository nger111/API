using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly string _connectionString;
        public WorkOrderRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<WorkOrder> GetAll() => throw new NotImplementedException();
        public WorkOrder? GetById(int id) => throw new NotImplementedException();
        public void Add(WorkOrder workOrder) => throw new NotImplementedException();
        public void Update(WorkOrder workOrder) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}