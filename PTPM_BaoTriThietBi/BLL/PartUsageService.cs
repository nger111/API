using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class PartUsageService : IPartUsageService
    {
        private readonly IPartUsageRepository _repository;

        public PartUsageService(IPartUsageRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<PartUsage> GetAll() => _repository.GetAll();

        public PartUsage? GetById(int id) => _repository.GetById(id);

        public void Add(PartUsage partUsage) => _repository.Add(partUsage);

        public void Update(PartUsage partUsage) => _repository.Update(partUsage);

        public void Delete(int id) => _repository.Delete(id);
    }
}