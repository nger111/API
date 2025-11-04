using Model;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public partial interface IWarrantiesRepository
    {
        bool Create(Warranties model);
        Warranties GetDatabyID(string id);
        List<Warranties> GetDataAll();
        bool Update(Warranties model);
        bool Delete(int warrantyId);
    }
}