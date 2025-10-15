using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class WarrantyRepository : IWarrantyRepository
    {
        private readonly string _connectionString;
        public WarrantyRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<Warranty> GetAll() => throw new NotImplementedException();
        public Warranty? GetById(int id) => throw new NotImplementedException();
        public void Add(Warranty warranty) => throw new NotImplementedException();
        public void Update(Warranty warranty) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}