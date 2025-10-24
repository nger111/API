using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class LinhKienRepository : ILinhKienRepository
    {
        private readonly string _chuoiKetNoi;
        public LinhKienRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<LinhKien> LayTatCa()
        {
            var ds = new List<LinhKien>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Parts", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new LinhKien
                {
                    MaLinhKien = rd.GetInt32(rd.GetOrdinal("PartID")),
                    TenLinhKien = rd.GetString(rd.GetOrdinal("PartName")),
                    MaLinhKienCode = rd.IsDBNull(rd.GetOrdinal("PartCode")) ? null : rd.GetString(rd.GetOrdinal("PartCode")),
                    SoLuongTon = rd.GetInt32(rd.GetOrdinal("StockQuantity")),
                    DonGia = rd.IsDBNull(rd.GetOrdinal("UnitPrice")) ? null : rd.GetDecimal(rd.GetOrdinal("UnitPrice")),
                    ViTri = rd.IsDBNull(rd.GetOrdinal("Location")) ? null : rd.GetString(rd.GetOrdinal("Location"))
                });
            }
            return ds;
        }

        public LinhKien? LayTheoMa(int maLinhKien)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Parts WHERE PartID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maLinhKien);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new LinhKien
            {
                MaLinhKien = rd.GetInt32(rd.GetOrdinal("PartID")),
                TenLinhKien = rd.GetString(rd.GetOrdinal("PartName")),
                MaLinhKienCode = rd.IsDBNull(rd.GetOrdinal("PartCode")) ? null : rd.GetString(rd.GetOrdinal("PartCode")),
                SoLuongTon = rd.GetInt32(rd.GetOrdinal("StockQuantity")),
                DonGia = rd.IsDBNull(rd.GetOrdinal("UnitPrice")) ? null : rd.GetDecimal(rd.GetOrdinal("UnitPrice")),
                ViTri = rd.IsDBNull(rd.GetOrdinal("Location")) ? null : rd.GetString(rd.GetOrdinal("Location"))
            };
        }

        public void Them(LinhKien lk)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO Parts (PartName, PartCode, StockQuantity, UnitPrice, Location)
                                             VALUES (@PartName, @PartCode, @StockQuantity, @UnitPrice, @Location)", conn);
            cmd.Parameters.AddWithValue("@PartName", lk.TenLinhKien);
            cmd.Parameters.AddWithValue("@PartCode", (object?)lk.MaLinhKienCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StockQuantity", lk.SoLuongTon);
            cmd.Parameters.AddWithValue("@UnitPrice", (object?)lk.DonGia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Location", (object?)lk.ViTri ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(LinhKien lk)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE Parts SET PartName=@PartName, PartCode=@PartCode, StockQuantity=@StockQuantity, 
                                             UnitPrice=@UnitPrice, Location=@Location WHERE PartID=@PartID", conn);
            cmd.Parameters.AddWithValue("@PartID", lk.MaLinhKien);
            cmd.Parameters.AddWithValue("@PartName", lk.TenLinhKien);
            cmd.Parameters.AddWithValue("@PartCode", (object?)lk.MaLinhKienCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StockQuantity", lk.SoLuongTon);
            cmd.Parameters.AddWithValue("@UnitPrice", (object?)lk.DonGia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Location", (object?)lk.ViTri ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maLinhKien)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM Parts WHERE PartID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maLinhKien);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}