using Model;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL
{
    public class AssetRepository : IAssetRepository
    {
        private readonly string _connectionString;
        public AssetRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<Asset> GetAll() => throw new NotImplementedException();
        public Asset? GetById(int id) => throw new NotImplementedException();
        public void Add(Asset asset) => throw new NotImplementedException();
        public void Update(Asset asset) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}