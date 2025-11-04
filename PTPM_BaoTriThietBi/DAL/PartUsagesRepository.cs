using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class PartUsagesRepository : IPartUsagesRepository
    {
        private IDatabaseHelper _dbHelper;
        public PartUsagesRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(PartUsages model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_partusages_create",
                    "@WorkOrderID", model.WorkOrderID,
                    "@PartID", model.PartID,
                    "@QuantityUsed", model.QuantityUsed
                );

                if ((result != null && !string.IsNullOrEmpty(result.ToString())) || !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(Convert.ToString(result) + msgError);
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        public PartUsages GetDatabyID(string id)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_partusages_get_by_id",
                    "@PartUsageID", id);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<PartUsages>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PartUsages> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_partusages_get_all");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                List<PartUsages> list = new List<PartUsages>();
                foreach (DataRow row in dt.Rows)
                {
                    var pu = new PartUsages
                    {
                        PartUsageID = Convert.ToInt32(row["PartUsageID"]),
                        WorkOrderID = Convert.ToInt32(row["WorkOrderID"]),
                        PartID = Convert.ToInt32(row["PartID"]),
                        QuantityUsed = Convert.ToInt32(row["QuantityUsed"]),
                        CreatedAt = row.Table.Columns.Contains("CreatedAt") && row["CreatedAt"] != DBNull.Value
                            ? Convert.ToDateTime(row["CreatedAt"])
                            : default
                    };

                    if (row.Table.Columns.Contains("PartName") && row["PartName"] != DBNull.Value)
                        pu.PartName = row["PartName"]?.ToString();

                    list.Add(pu);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(PartUsages model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_partusages_update",
                    "@PartUsageID", model.PartUsageID,
                    "@WorkOrderID", model.WorkOrderID,
                    "@PartID", model.PartID,
                    "@QuantityUsed", model.QuantityUsed
                );

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                    return rows > 0;

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Delete(int partUsageId)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_partusages_delete",
                    "@PartUsageID", partUsageId
                );

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                    return rows > 0;

                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}