using Model;

namespace DAL.Interfaces
{
    public interface IPartUsageRepository
    {
        IEnumerable<PartUsage> GetAll();
        PartUsage? GetById(int id);
        void Add(PartUsage partUsage);
        void Update(PartUsage partUsage);
        void Delete(int id);
    }
}