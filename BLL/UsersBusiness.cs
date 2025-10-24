using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public partial class UsersBusiness : IUsersBusiness
    {
        private readonly IUsersRepository _res;
        public UsersBusiness(IUsersRepository res)
        {
            _res = res;
        }

        public bool Create(Users model) => _res.Create(model);
        public Users GetDatabyID(string id) => _res.GetDatabyID(id);
        public List<Users> GetDataAll() => _res.GetDataAll();
        public bool Update(Users model) => _res.Update(model);
        public bool Delete(int userId) => _res.Delete(userId);
    }
}