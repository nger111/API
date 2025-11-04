using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class WarrantiesBusiness : IWarrantiesBusiness
    {
        private readonly IWarrantiesRepository _repo;

        public WarrantiesBusiness(IWarrantiesRepository repo)
        {
            _repo = repo;
        }

        public bool Create(Warranties model) => _repo.Create(model);
        public Warranties GetDatabyID(string id) => _repo.GetDatabyID(id);
        public List<Warranties> GetDataAll() => _repo.GetDataAll();
        public bool Update(Warranties model) => _repo.Update(model);
        public bool Delete(int warrantyId) => _repo.Delete(warrantyId);
    }
}