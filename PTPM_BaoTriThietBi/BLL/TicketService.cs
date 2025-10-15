using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Ticket> GetAll() => _repository.GetAll();

        public Ticket? GetById(int id) => _repository.GetById(id);

        public void Add(Ticket ticket) => _repository.Add(ticket);

        public void Update(Ticket ticket) => _repository.Update(ticket);

        public void Delete(int id) => _repository.Delete(id);
    }
}