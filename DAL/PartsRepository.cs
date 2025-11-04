using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class PartsRepository : IPartsRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public PartsRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(Parts model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_parts_create",
                    "@PartName", model.PartName,
                    "@PartCode", (object?)model.PartCode ?? DBNull.Value,
                    "@StockQuantity", model.StockQuantity,
                    "@UnitPrice", (object?)model.UnitPrice ?? DBNull.Value,
                    "@Location", (object?)model.Location ?? DBNull.Value,
                    "@UsageStatus", (object?)model.UsageStatus ?? DBNull.Value
                );
                if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
                if (result != null && int.TryParse(Convert.ToString(result), out var newId))
                    model.PartID = newId;
                return true;
            }
            catch { throw; }
        }

        public Parts GetDatabyID(string id)
        {
            string msgError = "";
            try
            {
                if (!int.TryParse(id, out var partId))
                    throw new ArgumentException("PartID phải là số nguyên.", nameof(id));

                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_parts_get_by_id",
                    "@PartID", partId);
                if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
                return dt.ConvertTo<Parts>().FirstOrDefault();
            }
            catch { throw; }
        }

        public List<Parts> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_parts_get_all");
                if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);

                // Có thể dùng ConvertTo nếu tên cột khớp property
                return (List<Parts>)dt.ConvertTo<Parts>();
            }
            catch { throw; }
        }

        public bool Update(Parts model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_parts_update",
                    "@PartID", model.PartID,
                    "@PartName", model.PartName,
                    "@PartCode", (object?)model.PartCode ?? DBNull.Value,
                    "@StockQuantity", model.StockQuantity,
                    "@UnitPrice", (object?)model.UnitPrice ?? DBNull.Value,
                    "@Location", (object?)model.Location ?? DBNull.Value,
                    "@UsageStatus", (object?)model.UsageStatus ?? DBNull.Value
                );
                if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
                if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                    return rows > 0;
                return true;
            }
            catch { throw; }
        }

        public bool Delete(int partId)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_parts_delete",
                    "@PartID", partId);
                if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
                if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                    return rows > 0;
                return false;
            }
            catch { throw; }
        }
    }
}