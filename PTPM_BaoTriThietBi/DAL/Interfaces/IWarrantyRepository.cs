using Model;

namespace DAL.Interfaces
{
    public interface IWarrantyRepository
    {
        IEnumerable<Warranty> GetAll();
        Warranty? GetById(int id);
        void Add(Warranty warranty);
        void Update(Warranty warranty);
        void Delete(int id);
    }
}