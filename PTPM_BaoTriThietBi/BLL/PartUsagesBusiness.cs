using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class PartUsagesBusiness : IPartUsagesBusiness
    {
        private readonly IPartUsagesRepository _repo;
        public PartUsagesBusiness(IPartUsagesRepository repo)
        {
            _repo = repo;
        }

        public bool Create(PartUsages model) => _repo.Create(model);
        public PartUsages GetDatabyID(string id) => _repo.GetDatabyID(id);
        public List<PartUsages> GetDataAll() => _repo.GetDataAll();
        public bool Update(PartUsages model) => _repo.Update(model);
        public bool Delete(int partUsageId) => _repo.Delete(partUsageId);
    }
}