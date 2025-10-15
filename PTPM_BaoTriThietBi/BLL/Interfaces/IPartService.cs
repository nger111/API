using Model;

namespace BLL.Interfaces
{
    public interface IPartService
    {
        IEnumerable<Part> GetAll();
        Part? GetById(int id);
        void Add(Part part);
        void Update(Part part);
        void Delete(int id);
    }
}