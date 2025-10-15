using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class PartUsageRepository : IPartUsageRepository
    {
        private readonly string _connectionString;
        public PartUsageRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<PartUsage> GetAll() => throw new NotImplementedException();
        public PartUsage? GetById(int id) => throw new NotImplementedException();
        public void Add(PartUsage partUsage) => throw new NotImplementedException();
        public void Update(PartUsage partUsage) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}