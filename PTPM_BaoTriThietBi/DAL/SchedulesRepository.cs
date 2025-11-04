using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class SchedulesRepository : ISchedulesRepository
    {
        private IDatabaseHelper _dbHelper;
        public SchedulesRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(Schedules model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_schedules_create",
                    "@AssetID", model.AssetID,
                    "@MaintenanceType", model.MaintenanceType,
                    "@NextMaintenanceDate", model.NextMaintenanceDate,
                    "@LastMaintenanceDate", (object?)model.LastMaintenanceDate ?? DBNull.Value,
                    "@Checklist", (object?)model.Checklist ?? DBNull.Value,
                    "@UsageStatus", model.UsageStatus
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

        public Schedules GetDatabyID(string id)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_schedules_get_by_id",
                     "@ScheduleID", id);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<Schedules>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Schedules> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_schedules_get_all");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                List<Schedules> list = new List<Schedules>();
                foreach (DataRow row in dt.Rows)
                {
                    var s = new Schedules
                    {
                        ScheduleID = Convert.ToInt32(row["ScheduleID"]),
                        AssetID = Convert.ToInt32(row["AssetID"]),
                        MaintenanceType = row["MaintenanceType"]?.ToString() ?? string.Empty,
                        NextMaintenanceDate = Convert.ToDateTime(row["NextMaintenanceDate"]),
                        LastMaintenanceDate = row["LastMaintenanceDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["LastMaintenanceDate"]),
                        Checklist = row.Table.Columns.Contains("Checklist") && row["Checklist"] != DBNull.Value ? row["Checklist"]?.ToString() : null,
                        UsageStatus = row["UsageStatus"]?.ToString() ?? "Active"
                    };

                    if (row.Table.Columns.Contains("AssetName") && row["AssetName"] != DBNull.Value)
                        s.AssetName = row["AssetName"]?.ToString();

                    list.Add(s);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(Schedules model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_schedules_update",
                    "@ScheduleID", model.ScheduleID,
                    "@AssetID", model.AssetID,
                    "@MaintenanceType", model.MaintenanceType,
                    "@NextMaintenanceDate", model.NextMaintenanceDate,
                    "@LastMaintenanceDate", (object?)model.LastMaintenanceDate ?? DBNull.Value,
                    "@Checklist", (object?)model.Checklist ?? DBNull.Value,
                    "@UsageStatus", model.UsageStatus
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

        public bool Delete(int scheduleId)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_schedules_delete",
                    "@ScheduleID", scheduleId
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