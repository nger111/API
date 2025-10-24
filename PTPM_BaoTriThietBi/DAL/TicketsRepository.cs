using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DAL
{
    public partial class TicketsResponsitory : IticketsRepository
    {
        private IDatabaseHelper _dbHelper;
        public TicketsResponsitory(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(Tickets model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_tickets_create",
                    "@AssetID", model.AssetID,
                    "@CreatedBy", model.CreatedBy,
                    "@AssignedTo", model.AssignedTo,
                    "@Priority", model.Priority,
                    "@SLAHours", model.SLAHours,
                    "@IssueDescription", model.IssueDescription,
                    "@Status", model.Status
                );

                if ((result != null && !string.IsNullOrEmpty(result.ToString())) || !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(Convert.ToString(result) + msgError);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Tickets GetDatabyID(string id)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_ticket_get_by_id",
                     "@AssetID", id);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<Tickets>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Tickets> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_tickets_get_all");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                List<Tickets> list = new List<Tickets>();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    Tickets ticket = new Tickets();
                    ticket.TicketID = Convert.ToInt32(row["TicketID"]);
                    ticket.AssetID = Convert.ToInt32(row["AssetID"]);
                    ticket.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                    ticket.AssignedTo = row["AssignedTo"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["AssignedTo"]);
                    ticket.Priority = row["Priority"].ToString();
                    ticket.SLAHours = row["SLAHours"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["SLAHours"]);
                    ticket.IssueDescription = row["IssueDescription"].ToString();
                    ticket.Status = row["Status"].ToString();
                    ticket.CreatedAt = Convert.ToDateTime(row["CreatedAt"]);

                    list.Add(ticket);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Implement Update to satisfy interface and call stored procedure
        public bool Update(Tickets model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_tickets_update",
                    "@TicketID", model.TicketID,
                    "@AssetID", model.AssetID,
                    "@AssignedTo", (object?)model.AssignedTo ?? DBNull.Value,
                    "@Priority", model.Priority,
                    "@SLAHours", (object?)model.SLAHours ?? DBNull.Value,
                    "@IssueDescription", model.IssueDescription ?? string.Empty,
                    "@Status", model.Status ?? string.Empty
                );

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                    return rows > 0;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

            // NEW: Xóa ticket
        public bool Delete(int ticketId)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_tickets_delete",
                    "@TicketID", ticketId
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
