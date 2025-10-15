using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class PartRepository : IPartRepository
    {
        private readonly string _connectionString;
        public PartRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<Part> GetAll() => throw new NotImplementedException();
        public Part? GetById(int id) => throw new NotImplementedException();
        public void Add(Part part) => throw new NotImplementedException();
        public void Update(Part part) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}