using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _repository;

        public PartService(IPartRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Part> GetAll() => _repository.GetAll();

        public Part? GetById(int id) => _repository.GetById(id);

        public void Add(Part part) => _repository.Add(part);

        public void Update(Part part) => _repository.Update(part);

        public void Delete(int id) => _repository.Delete(id);
    }
}