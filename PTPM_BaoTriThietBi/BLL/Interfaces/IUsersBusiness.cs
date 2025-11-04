using Model;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public partial interface IUsersBusiness
    {
        bool Create(Users model);
        Users GetDatabyID(string id);
        List<Users> GetDataAll();
        bool Update(Users model);
        bool Delete(int userId);
        Users Authenticate(string email, string passwordHash);
    }
}