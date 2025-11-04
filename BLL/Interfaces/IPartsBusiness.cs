using Model;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public partial interface IPartsBusiness
    {
        bool Create(Parts model);
        Parts GetDatabyID(string id);
        List<Parts> GetDataAll();
        bool Update(Parts model);
        bool Delete(int partId);
    }
}