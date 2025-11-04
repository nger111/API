using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;

namespace DAL
{
    public partial class UsersRepository : IUsersRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public UsersRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(Users model)
        {
            var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out var msgError, "sp_users_create",
                "@FullName", model.FullName,
                "@Email", model.Email,
                "@PasswordHash", model.PasswordHash,
                "@Role", model.Role,
                "@Phone", (object?)model.Phone ?? DBNull.Value,
                "@SkillLevel", (object?)model.SkillLevel ?? DBNull.Value,
                "@Certifications", (object?)model.Certifications ?? DBNull.Value
            );
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
            if (result != null && int.TryParse(Convert.ToString(result), out var newId))
                model.UserID = newId;
            return true;
        }

        public Users GetDatabyID(string id)
        {
            if (!int.TryParse(id, out var userId))
                throw new ArgumentException("UserID must be an integer.", nameof(id));

            var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out var msgError, "sp_users_get_by_id",
                "@UserID", userId);
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);

            return dt.ConvertTo<Users>().FirstOrDefault();
        }

        public List<Users> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_users_get_all");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                var list = new List<Users>();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    var user = new Users
                    {
                        UserID = Convert.ToInt32(row["UserID"]),
                        FullName = row["FullName"]?.ToString() ?? string.Empty,
                        Email = row["Email"]?.ToString() ?? string.Empty,
                        PasswordHash = row["PasswordHash"]?.ToString() ?? string.Empty,
                        Role = row["Role"]?.ToString() ?? string.Empty,
                        Phone = row["Phone"] == DBNull.Value ? null : row["Phone"]?.ToString(),
                        SkillLevel = row["SkillLevel"] == DBNull.Value ? null : row["SkillLevel"]?.ToString(),
                        Certifications = row["Certifications"] == DBNull.Value ? null : row["Certifications"]?.ToString(),
                        CreatedAt = Convert.ToDateTime(row["CreatedAt"])
                    };
                    list.Add(user);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(Users model)
        {
            var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out var msgError, "sp_users_update",
                "@UserID", model.UserID,
                "@FullName", model.FullName,
                "@Email", model.Email,
                "@Role", model.Role,
                "@Phone", (object?)model.Phone ?? DBNull.Value,
                "@SkillLevel", (object?)model.SkillLevel ?? DBNull.Value,
                "@Certifications", (object?)model.Certifications ?? DBNull.Value
            );
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
            if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                return rows > 0;
            return true;
        }

        public bool Delete(int userId)
        {
            var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out var msgError, "sp_users_delete",
                "@UserID", userId);
            if (!string.IsNullOrEmpty(msgError)) throw new Exception(msgError);
            if (result != null && int.TryParse(Convert.ToString(result), out var rows))
                return rows > 0;
            return false;
        }

        public Users Authenticate(string email, string passwordHash)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_users_authenticate",
                    "@Email", email,
                    "@PasswordHash", passwordHash);
                
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<Users>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}