using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<User> GetAll() => throw new NotImplementedException();
        public User? GetById(int id) => throw new NotImplementedException();
        public void Add(User user) => throw new NotImplementedException();
        public void Update(User user) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}