using Model;
using System.Collections.Generic;

namespace DAL
{
    public partial interface IPartsRepository
    {
        bool Create(Parts model);
        Parts GetDatabyID(string id);
        List<Parts> GetDataAll();
        bool Update(Parts model);
        bool Delete(int partId);
    }
}