using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class WarrantyService : IWarrantyService
    {
        private readonly IWarrantyRepository _repository;

        public WarrantyService(IWarrantyRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Warranty> GetAll() => _repository.GetAll();

        public Warranty? GetById(int id) => _repository.GetById(id);

        public void Add(Warranty warranty) => _repository.Add(warranty);

        public void Update(Warranty warranty) => _repository.Update(warranty);

        public void Delete(int id) => _repository.Delete(id);
    }
}