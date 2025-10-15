using Model;

namespace BLL.Interfaces
{
    public interface ITicketService
    {
        IEnumerable<Ticket> GetAll();
        Ticket? GetById(int id);
        void Add(Ticket ticket);
        void Update(Ticket ticket);
        void Delete(int id);
    }
}