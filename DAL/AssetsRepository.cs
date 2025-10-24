using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;
using System.Linq;

namespace DAL
{
    public partial class AssetsRepository : IAssetsRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public AssetsRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(Assets model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_Assets_create",
                    "@AssetName", model.AssetName,
                    "@SerialNumber", model.SerialNumber,
                    "@Location", (object?)model.Location ?? DBNull.Value,
                    "@PurchaseDate", (object?)model.PurchaseDate ?? DBNull.Value,
                    "@Status", model.Status,
                    "@UsageStatus", model.UsageStatus ?? "Active"  // ← Đảm bảo dòng này tồn tại
                );

                if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);

                if (result != null && int.TryParse(Convert.ToString(result), out var newId))
                    model.AssetID = newId;

                return true;
            }
            catch
            {
                throw;
            }
        }

        public Assets GetDatabyID(string id)
        {
            if (!int.TryParse(id, out var assetId))
                throw new ArgumentException("AssetID must be an integer.", nameof(id));

            var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out var msgError, "sp_assets_get_by_id",
                "@AssetID", assetId);
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);

            return dt.ConvertTo<Assets>().FirstOrDefault();
        }

        public System.Collections.Generic.List<Assets> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_assets_get_all");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                var list = new List<Assets>();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var asset = new Assets
                    {
                        AssetID = Convert.ToInt32(row["AssetID"]),
                        AssetName = row["AssetName"]?.ToString() ?? string.Empty,
                        SerialNumber = row["SerialNumber"]?.ToString() ?? string.Empty,
                        Location = row["Location"] == DBNull.Value ? null : row["Location"]?.ToString(),
                        PurchaseDate = row["PurchaseDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["PurchaseDate"]),
                        Status = row["Status"]?.ToString() ?? string.Empty
                    };

                    if (row.Table.Columns.Contains("CreatedAt") && row["CreatedAt"] != DBNull.Value)
                        asset.CreatedAt = Convert.ToDateTime(row["CreatedAt"]);

                    if (row.Table.Columns.Contains("UsageStatus") && row["UsageStatus"] != DBNull.Value)
                        asset.UsageStatus = row["UsageStatus"]?.ToString();

                    list.Add(asset);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(Assets model)
        {
            var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out var msgError, "sp_Assets_update",
                "@AssetID", model.AssetID,
                "@AssetName", model.AssetName,
                "@SerialNumber", model.SerialNumber,
                "@Location", (object?)model.Location ?? DBNull.Value,
                "@PurchaseDate", (object?)model.PurchaseDate ?? DBNull.Value,
                "@Status", model.Status,
                "@UsageStatus", (object?)model.UsageStatus ?? DBNull.Value
            );
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
            if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                return rows > 0;
            return true;
        }

        public bool Delete(int assetId)
        {
            var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out var msgError, "sp_Assets_delete",
                "@AssetID", assetId);
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
            if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                return rows > 0;
            return false;
        }
    }
}