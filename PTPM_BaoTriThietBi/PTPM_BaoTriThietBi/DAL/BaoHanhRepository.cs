using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class BaoHanhRepository : IBaoHanhRepository
    {
        private readonly string _chuoiKetNoi;
        public BaoHanhRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<BaoHanh> LayTatCa()
        {
            var ds = new List<BaoHanh>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Warranties", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new BaoHanh
                {
                    MaBaoHanh = rd.GetInt32(rd.GetOrdinal("WarrantyID")),
                    MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                    NhaCungCapBaoHanh = rd.IsDBNull(rd.GetOrdinal("WarrantyProvider")) ? null : rd.GetString(rd.GetOrdinal("WarrantyProvider")),
                    NgayBatDau = rd.IsDBNull(rd.GetOrdinal("StartDate")) ? null : rd.GetDateTime(rd.GetOrdinal("StartDate")),
                    NgayKetThuc = rd.IsDBNull(rd.GetOrdinal("EndDate")) ? null : rd.GetDateTime(rd.GetOrdinal("EndDate")),
                    DieuKhoan = rd.IsDBNull(rd.GetOrdinal("Terms")) ? null : rd.GetString(rd.GetOrdinal("Terms"))
                });
            }
            return ds;
        }

        public BaoHanh? LayTheoMa(int maBaoHanh)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Warranties WHERE WarrantyID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maBaoHanh);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new BaoHanh
            {
                MaBaoHanh = rd.GetInt32(rd.GetOrdinal("WarrantyID")),
                MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                NhaCungCapBaoHanh = rd.IsDBNull(rd.GetOrdinal("WarrantyProvider")) ? null : rd.GetString(rd.GetOrdinal("WarrantyProvider")),
                NgayBatDau = rd.IsDBNull(rd.GetOrdinal("StartDate")) ? null : rd.GetDateTime(rd.GetOrdinal("StartDate")),
                NgayKetThuc = rd.IsDBNull(rd.GetOrdinal("EndDate")) ? null : rd.GetDateTime(rd.GetOrdinal("EndDate")),
                DieuKhoan = rd.IsDBNull(rd.GetOrdinal("Terms")) ? null : rd.GetString(rd.GetOrdinal("Terms"))
            };
        }

        public void Them(BaoHanh baoHanh)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO Warranties (AssetID, WarrantyProvider, StartDate, EndDate, Terms)
                                             VALUES (@AssetID, @WarrantyProvider, @StartDate, @EndDate, @Terms)", conn);
            cmd.Parameters.AddWithValue("@AssetID", baoHanh.MaTaiSan);
            cmd.Parameters.AddWithValue("@WarrantyProvider", (object?)baoHanh.NhaCungCapBaoHanh ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StartDate", (object?)baoHanh.NgayBatDau ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object?)baoHanh.NgayKetThuc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Terms", (object?)baoHanh.DieuKhoan ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(BaoHanh baoHanh)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE Warranties SET AssetID=@AssetID, WarrantyProvider=@WarrantyProvider, StartDate=@StartDate,
                                             EndDate=@EndDate, Terms=@Terms WHERE WarrantyID=@WarrantyID", conn);
            cmd.Parameters.AddWithValue("@WarrantyID", baoHanh.MaBaoHanh);
            cmd.Parameters.AddWithValue("@AssetID", baoHanh.MaTaiSan);
            cmd.Parameters.AddWithValue("@WarrantyProvider", (object?)baoHanh.NhaCungCapBaoHanh ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StartDate", (object?)baoHanh.NgayBatDau ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object?)baoHanh.NgayKetThuc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Terms", (object?)baoHanh.DieuKhoan ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maBaoHanh)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM Warranties WHERE WarrantyID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maBaoHanh);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}