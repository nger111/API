using Model;

namespace BLL.Interfaces
{
    public interface IAssetService
    {
        IEnumerable<Asset> GetAll();
        Asset? GetById(int id);
        void Add(Asset asset);
        void Update(Asset asset);
        void Delete(int id);
    }
}