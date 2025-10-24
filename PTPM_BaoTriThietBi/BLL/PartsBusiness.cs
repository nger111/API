using BLL.Interfaces;
using DAL;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class PartsBusiness : IPartsBusiness
    {
        private readonly IPartsRepository _repo;
        public PartsBusiness(IPartsRepository repo)
        {
            _repo = repo;
        }

        public bool Create(Parts model) => _repo.Create(model);
        public Parts GetDatabyID(string id) => _repo.GetDatabyID(id);
        public List<Parts> GetDataAll() => _repo.GetDataAll();
        public bool Update(Parts model) => _repo.Update(model);
        public bool Delete(int partId) => _repo.Delete(partId);
    }
}