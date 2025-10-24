using BLL.Interfaces;
using DAL;
using DAL.Interfaces;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public partial class AssetsBusiness : IAssetsBusiness
    {
        private readonly IAssetsRepository _res;
        public AssetsBusiness(IAssetsRepository res)
        {
            _res = res;
        }

        public bool Create(Assets model)
        {
            return _res.Create(model);
        }
        public Assets GetDatabyID(string id)
        {
           return _res.GetDatabyID(id); 
        }

        public List<Assets> GetDataAll()
        {
           return _res.GetDataAll(); 
        }

        public bool Update(Assets model)
        {
           return _res.Update(model);
        }
        public bool Delete(int assetId)
        {
            return _res.Delete(assetId);
        }
    }
}



