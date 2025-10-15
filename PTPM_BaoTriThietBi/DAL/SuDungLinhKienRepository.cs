using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class SuDungLinhKienRepository : ISuDungLinhKienRepository
    {
        private readonly string _chuoiKetNoi;
        public SuDungLinhKienRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<SuDungLinhKien> LayTatCa()
        {
            var ds = new List<SuDungLinhKien>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM PartUsages", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new SuDungLinhKien
                {
                    MaSuDungLinhKien = rd.GetInt32(rd.GetOrdinal("PartUsageID")),
                    MaLenhCongViec = rd.GetInt32(rd.GetOrdinal("WorkOrderID")),
                    MaLinhKien = rd.GetInt32(rd.GetOrdinal("PartID")),
                    SoLuongSuDung = rd.GetInt32(rd.GetOrdinal("QuantityUsed")),
                    ThoiGianSuDung = rd.GetDateTime(rd.GetOrdinal("UsedAt"))
                });
            }
            return ds;
        }

        public SuDungLinhKien? LayTheoMa(int maSuDungLinhKien)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM PartUsages WHERE PartUsageID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maSuDungLinhKien);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new SuDungLinhKien
            {
                MaSuDungLinhKien = rd.GetInt32(rd.GetOrdinal("PartUsageID")),
                MaLenhCongViec = rd.GetInt32(rd.GetOrdinal("WorkOrderID")),
                MaLinhKien = rd.GetInt32(rd.GetOrdinal("PartID")),
                SoLuongSuDung = rd.GetInt32(rd.GetOrdinal("QuantityUsed")),
                ThoiGianSuDung = rd.GetDateTime(rd.GetOrdinal("UsedAt"))
            };
        }

        public void Them(SuDungLinhKien sdlk)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO PartUsages (WorkOrderID, PartID, QuantityUsed, UsedAt)
                                             VALUES (@WorkOrderID, @PartID, @QuantityUsed, @UsedAt)", conn);
            cmd.Parameters.AddWithValue("@WorkOrderID", sdlk.MaLenhCongViec);
            cmd.Parameters.AddWithValue("@PartID", sdlk.MaLinhKien);
            cmd.Parameters.AddWithValue("@QuantityUsed", sdlk.SoLuongSuDung);
            cmd.Parameters.AddWithValue("@UsedAt", sdlk.ThoiGianSuDung);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(SuDungLinhKien sdlk)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE PartUsages SET WorkOrderID=@WorkOrderID, PartID=@PartID, 
                                             QuantityUsed=@QuantityUsed, UsedAt=@UsedAt WHERE PartUsageID=@PartUsageID", conn);
            cmd.Parameters.AddWithValue("@PartUsageID", sdlk.MaSuDungLinhKien);
            cmd.Parameters.AddWithValue("@WorkOrderID", sdlk.MaLenhCongViec);
            cmd.Parameters.AddWithValue("@PartID", sdlk.MaLinhKien);
            cmd.Parameters.AddWithValue("@QuantityUsed", sdlk.SoLuongSuDung);
            cmd.Parameters.AddWithValue("@UsedAt", sdlk.ThoiGianSuDung);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maSuDungLinhKien)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM PartUsages WHERE PartUsageID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maSuDungLinhKien);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}