using Model;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public partial interface IAssetsRepository
    {
        bool Create(Assets model);
        Assets GetDatabyID(string id);
        List<Assets> GetDataAll();
        bool Update(Assets model);
        bool Delete(int assetId);
    }
}