using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class WarrantiesRepository : IWarrantiesRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public WarrantiesRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(Warranties model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_Warranties_create",
                    "@AssetID", model.AssetID,
                    "@WarrantyProvider", (object?)model.WarrantyProvider ?? DBNull.Value,
                    "@StartDate", model.StartDate,
                    "@EndDate", (object?)model.EndDate ?? DBNull.Value,
                    "@Terms", (object?)model.Terms ?? DBNull.Value,
                    "@Status", model.Status
                );

                // Chỉ lỗi khi msgError có nội dung
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                // Nếu proc trả về ID mới thì gán lại cho model
                if (result != null && int.TryParse(Convert.ToString(result), out var newId))
                    model.WarrantyID = newId;

                return true;
            }
            catch
            {
                throw;
            }
        }

        public Warranties GetDatabyID(string id)
        {
            if (!int.TryParse(id, out var warrantyId))
                throw new ArgumentException("WarrantyID must be an integer.", nameof(id));

            var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out var msgError, "sp_Warranties_get_by_id",
                "@WarrantyID", warrantyId);
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);

            var entity = dt.ConvertTo<Warranties>().FirstOrDefault();
            if (entity == null && dt.Rows.Count > 0)
            {
                entity = MapRow(dt.Rows[0]);
            }
            return entity!;
        }

        public List<Warranties> GetDataAll()
        {
            var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out var msgError, "sp_Warranties_get_all");
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);

            var list = new List<Warranties>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRow(row));
            }
            return list;
        }

        public bool Update(Warranties model)
        {
            var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(
                out var msgError, "sp_Warranties_update",
                "@WarrantyID", model.WarrantyID,
                "@AssetID", model.AssetID,
                "@WarrantyProvider", (object?)model.WarrantyProvider ?? DBNull.Value,
                "@StartDate", model.StartDate,
                "@EndDate", (object?)model.EndDate ?? DBNull.Value,
                "@Terms", (object?)model.Terms ?? DBNull.Value,
                "@Status", model.Status
            );
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
            if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                return rows > 0;
            return true;
        }

        public bool Delete(int warrantyId)
        {
            var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(
                out var msgError, "sp_Warranties_delete",
                "@WarrantyID", warrantyId);
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
            if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                return rows > 0;
            return false;
        }

        private static Warranties MapRow(DataRow row)
        {
            var entity = new Warranties
            {
                WarrantyID = Convert.ToInt32(row["WarrantyID"]),
                AssetID = Convert.ToInt32(row["AssetID"]),
                WarrantyProvider = row.Table.Columns.Contains("WarrantyProvider") && row["WarrantyProvider"] != DBNull.Value ? row["WarrantyProvider"]?.ToString() : null,
                StartDate = Convert.ToDateTime(row["StartDate"]),
                EndDate = row["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["EndDate"]),
                Terms = row.Table.Columns.Contains("Terms") && row["Terms"] != DBNull.Value ? row["Terms"]?.ToString() : null,
                Status = row.Table.Columns.Contains("Status") && row["Status"] != DBNull.Value ? row["Status"]!.ToString()! : "Active",
                CreatedAt = row.Table.Columns.Contains("CreatedAt") && row["CreatedAt"] != DBNull.Value
                    ? Convert.ToDateTime(row["CreatedAt"])
                    : default
            };

            if (row.Table.Columns.Contains("AssetName") && row["AssetName"] != DBNull.Value)
                entity.AssetName = row["AssetName"]?.ToString();

            return entity;
        }
    }
}