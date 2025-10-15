using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class TicketRepository : ITicketRepository
    {
        private readonly string _connectionString;
        public TicketRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<Ticket> GetAll() => throw new NotImplementedException();
        public Ticket? GetById(int id) => throw new NotImplementedException();
        public void Add(Ticket ticket) => throw new NotImplementedException();
        public void Update(Ticket ticket) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}