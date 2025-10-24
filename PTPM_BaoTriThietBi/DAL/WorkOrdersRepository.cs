using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class WorkOrdersRepository : IWorkOrdersRepository
    {
        private IDatabaseHelper _dbHelper;
        public WorkOrdersRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(WorkOrders model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_workorders_create",
                    "@ScheduleID", (object?)model.ScheduleID ?? DBNull.Value,
                    "@TicketID", (object?)model.TicketID ?? DBNull.Value,
                    "@AssetID", model.AssetID,
                    "@AssignedTo", (object?)model.AssignedTo ?? DBNull.Value,
                    "@WorkType", model.WorkType,
                    "@Description", (object?)model.Description ?? DBNull.Value,
                    "@Status", model.Status,
                    "@UsageStatus", model.UsageStatus ?? "Active"
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

        public WorkOrders GetDatabyID(string id)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_workorders_get_by_id",
                     "@WorkOrderID", id);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<WorkOrders>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkOrders> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_workorders_get_all");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                List<WorkOrders> list = new List<WorkOrders>();
                foreach (DataRow row in dt.Rows)
                {
                    var wo = new WorkOrders
                    {
                        WorkOrderID = Convert.ToInt32(row["WorkOrderID"]),
                        ScheduleID = row["ScheduleID"] == DBNull.Value ? null : Convert.ToInt32(row["ScheduleID"]),
                        TicketID = row["TicketID"] == DBNull.Value ? null : Convert.ToInt32(row["TicketID"]),
                        AssetID = Convert.ToInt32(row["AssetID"]),
                        AssignedTo = row["AssignedTo"] == DBNull.Value ? null : Convert.ToInt32(row["AssignedTo"]),
                        WorkType = row["WorkType"]?.ToString() ?? string.Empty,
                        Description = row["Description"] == DBNull.Value ? null : row["Description"]?.ToString(),
                        Status = row["Status"]?.ToString() ?? string.Empty,
                        UsageStatus = row.Table.Columns.Contains("UsageStatus") && row["UsageStatus"] != DBNull.Value ? row["UsageStatus"]?.ToString() : "Active",
                        CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                        CompletedAt = row["CompletedAt"] == DBNull.Value ? null : Convert.ToDateTime(row["CompletedAt"])
                    };

                    if (row.Table.Columns.Contains("AssetName") && row["AssetName"] != DBNull.Value)
                        wo.AssetName = row["AssetName"]?.ToString();

                    if (row.Table.Columns.Contains("AssignedToName") && row["AssignedToName"] != DBNull.Value)
                        wo.AssignedToName = row["AssignedToName"]?.ToString();

                    list.Add(wo);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(WorkOrders model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_workorders_update",
                    "@WorkOrderID", model.WorkOrderID,
                    "@ScheduleID", (object?)model.ScheduleID ?? DBNull.Value,
                    "@TicketID", (object?)model.TicketID ?? DBNull.Value,
                    "@AssetID", model.AssetID,
                    "@AssignedTo", (object?)model.AssignedTo ?? DBNull.Value,
                    "@WorkType", model.WorkType,
                    "@Description", (object?)model.Description ?? DBNull.Value,
                    "@Status", model.Status,
                    "@CompletedAt", (object?)model.CompletedAt ?? DBNull.Value,
                    "@UsageStatus", model.UsageStatus ?? "Active"
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

        public bool Delete(int workOrderId)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_workorders_delete",
                    "@WorkOrderID", workOrderId
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