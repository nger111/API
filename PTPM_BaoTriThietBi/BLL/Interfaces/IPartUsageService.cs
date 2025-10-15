using Model;

namespace BLL.Interfaces
{
    public interface IPartUsageService
    {
        IEnumerable<PartUsage> GetAll();
        PartUsage? GetById(int id);
        void Add(PartUsage partUsage);
        void Update(PartUsage partUsage);
        void Delete(int id);
    }
}