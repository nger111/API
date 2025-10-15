using Model;

namespace DAL.Interfaces
{
    public interface IPartRepository
    {
        IEnumerable<Part> GetAll();
        Part? GetById(int id);
        void Add(Part part);
        void Update(Part part);
        void Delete(int id);
    }
}