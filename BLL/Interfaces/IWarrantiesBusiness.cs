using Model;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public partial interface IWarrantiesBusiness
    {
        bool Create(Warranties model);
        Warranties GetDatabyID(string id);
        List<Warranties> GetDataAll();
        bool Update(Warranties model);
        bool Delete(int warrantyId);
    }
}