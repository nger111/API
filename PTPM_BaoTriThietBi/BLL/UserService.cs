using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public interface IUserService
    {
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> GetAll() => _repository.GetAll();

        public User? GetById(int id) => _repository.GetById(id);

        public void Add(User user) => _repository.Add(user);

        public void Update(User user) => _repository.Update(user);

        public void Delete(int id) => _repository.Delete(id);
    }
}