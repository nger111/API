using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly string _chuoiKetNoi;
        public NguoiDungRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<NguoiDung> LayTatCa()
        {
            var ds = new List<NguoiDung>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Users", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new NguoiDung
                {
                    MaNguoiDung = rd.GetInt32(rd.GetOrdinal("UserID")),
                    HoTen = rd.GetString(rd.GetOrdinal("FullName")),
                    Email = rd.GetString(rd.GetOrdinal("Email")),
                    MatKhauHash = rd.GetString(rd.GetOrdinal("PasswordHash")),
                    VaiTro = rd.GetString(rd.GetOrdinal("Role")),
                    DienThoai = rd.IsDBNull(rd.GetOrdinal("Phone")) ? null : rd.GetString(rd.GetOrdinal("Phone")),
                    TrinhDoKyNang = rd.IsDBNull(rd.GetOrdinal("SkillLevel")) ? null : rd.GetString(rd.GetOrdinal("SkillLevel")),
                    ChungChi = rd.IsDBNull(rd.GetOrdinal("Certifications")) ? null : rd.GetString(rd.GetOrdinal("Certifications")),
                    NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
                });
            }
            return ds;
        }

        public NguoiDung? LayTheoMa(int maNguoiDung)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Users WHERE UserID = @id", conn);
            cmd.Parameters.AddWithValue("@id", maNguoiDung);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new NguoiDung
            {
                MaNguoiDung = rd.GetInt32(rd.GetOrdinal("UserID")),
                HoTen = rd.GetString(rd.GetOrdinal("FullName")),
                Email = rd.GetString(rd.GetOrdinal("Email")),
                MatKhauHash = rd.GetString(rd.GetOrdinal("PasswordHash")),
                VaiTro = rd.GetString(rd.GetOrdinal("Role")),
                DienThoai = rd.IsDBNull(rd.GetOrdinal("Phone")) ? null : rd.GetString(rd.GetOrdinal("Phone")),
                TrinhDoKyNang = rd.IsDBNull(rd.GetOrdinal("SkillLevel")) ? null : rd.GetString(rd.GetOrdinal("SkillLevel")),
                ChungChi = rd.IsDBNull(rd.GetOrdinal("Certifications")) ? null : rd.GetString(rd.GetOrdinal("Certifications")),
                NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
            };
        }

        public void Them(NguoiDung nguoiDung)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO Users (FullName, Email, PasswordHash, Role, Phone, SkillLevel, Certifications)
                                             VALUES (@FullName, @Email, @PasswordHash, @Role, @Phone, @SkillLevel, @Certifications)", conn);
            cmd.Parameters.AddWithValue("@FullName", nguoiDung.HoTen);
            cmd.Parameters.AddWithValue("@Email", nguoiDung.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", nguoiDung.MatKhauHash);
            cmd.Parameters.AddWithValue("@Role", nguoiDung.VaiTro);
            cmd.Parameters.AddWithValue("@Phone", (object?)nguoiDung.DienThoai ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SkillLevel", (object?)nguoiDung.TrinhDoKyNang ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Certifications", (object?)nguoiDung.ChungChi ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(NguoiDung nguoiDung)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE Users SET FullName=@FullName, Email=@Email, PasswordHash=@PasswordHash,
                                             Role=@Role, Phone=@Phone, SkillLevel=@SkillLevel, Certifications=@Certifications
                                             WHERE UserID=@UserID", conn);
            cmd.Parameters.AddWithValue("@UserID", nguoiDung.MaNguoiDung);
            cmd.Parameters.AddWithValue("@FullName", nguoiDung.HoTen);
            cmd.Parameters.AddWithValue("@Email", nguoiDung.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", nguoiDung.MatKhauHash);
            cmd.Parameters.AddWithValue("@Role", nguoiDung.VaiTro);
            cmd.Parameters.AddWithValue("@Phone", (object?)nguoiDung.DienThoai ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SkillLevel", (object?)nguoiDung.TrinhDoKyNang ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Certifications", (object?)nguoiDung.ChungChi ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maNguoiDung)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM Users WHERE UserID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maNguoiDung);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}