using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly string _connectionString;
        public ScheduleRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<Schedule> GetAll() => throw new NotImplementedException();
        public Schedule? GetById(int id) => throw new NotImplementedException();
        public void Add(Schedule schedule) => throw new NotImplementedException();
        public void Update(Schedule schedule) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}