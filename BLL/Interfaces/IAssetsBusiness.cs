using Model;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public partial interface IAssetsBusiness
    {
        bool Create(Assets model);
        Assets GetDatabyID(string id);
        List<Assets> GetDataAll();
        bool Update(Assets model);
        bool Delete(int assetId);
    }
}