using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _repository;

        public AssetService(IAssetRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Asset> GetAll() => _repository.GetAll();

        public Asset? GetById(int id) => _repository.GetById(id);

        public void Add(Asset asset) => _repository.Add(asset);

        public void Update(Asset asset) => _repository.Update(asset);

        public void Delete(int id) => _repository.Delete(id);
    }
}