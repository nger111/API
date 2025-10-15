using Model;

namespace BLL.Interfaces
{
    public interface IWarrantyService
    {
        IEnumerable<Warranty> GetAll();
        Warranty? GetById(int id);
        void Add(Warranty warranty);
        void Update(Warranty warranty);
        void Delete(int id);
    }
}