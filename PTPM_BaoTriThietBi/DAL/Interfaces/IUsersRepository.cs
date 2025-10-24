using Model;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public partial interface IUsersRepository
    {
        bool Create(Users model);
        Users GetDatabyID(string id);
        List<Users> GetDataAll();
        bool Update(Users model);
        bool Delete(int userId);
    }
}