using Model;

namespace DAL.Interfaces
{
    public interface IAssetRepository
    {
        IEnumerable<Asset> GetAll();
        Asset? GetById(int id);
        void Add(Asset asset);
        void Update(Asset asset);
        void Delete(int id);
    }
}