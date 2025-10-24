using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class LichBaoTriRepository : ILichBaoTriRepository
    {
        private readonly string _chuoiKetNoi;
        public LichBaoTriRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<LichBaoTri> LayTatCa()
        {
            var ds = new List<LichBaoTri>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Schedules", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new LichBaoTri
                {
                    MaLichBaoTri = rd.GetInt32(rd.GetOrdinal("ScheduleID")),
                    MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                    LoaiBaoTri = rd.GetString(rd.GetOrdinal("MaintenanceType")),
                    NgayBaoTriTiepTheo = rd.GetDateTime(rd.GetOrdinal("NextMaintenanceDate")),
                    NgayBaoTriGanNhat = rd.IsDBNull(rd.GetOrdinal("LastMaintenanceDate")) ? null : rd.GetDateTime(rd.GetOrdinal("LastMaintenanceDate")),
                    DanhSachKiemTra = rd.IsDBNull(rd.GetOrdinal("Checklist")) ? null : rd.GetString(rd.GetOrdinal("Checklist"))
                });
            }
            return ds;
        }

        public LichBaoTri? LayTheoMa(int maLichBaoTri)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Schedules WHERE ScheduleID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maLichBaoTri);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new LichBaoTri
            {
                MaLichBaoTri = rd.GetInt32(rd.GetOrdinal("ScheduleID")),
                MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                LoaiBaoTri = rd.GetString(rd.GetOrdinal("MaintenanceType")),
                NgayBaoTriTiepTheo = rd.GetDateTime(rd.GetOrdinal("NextMaintenanceDate")),
                NgayBaoTriGanNhat = rd.IsDBNull(rd.GetOrdinal("LastMaintenanceDate")) ? null : rd.GetDateTime(rd.GetOrdinal("LastMaintenanceDate")),
                DanhSachKiemTra = rd.IsDBNull(rd.GetOrdinal("Checklist")) ? null : rd.GetString(rd.GetOrdinal("Checklist"))
            };
        }

        public void Them(LichBaoTri lichBaoTri)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO Schedules (AssetID, MaintenanceType, NextMaintenanceDate, LastMaintenanceDate, Checklist)
                                             VALUES (@AssetID, @MaintenanceType, @NextMaintenanceDate, @LastMaintenanceDate, @Checklist)", conn);
            cmd.Parameters.AddWithValue("@AssetID", lichBaoTri.MaTaiSan);
            cmd.Parameters.AddWithValue("@MaintenanceType", lichBaoTri.LoaiBaoTri);
            cmd.Parameters.AddWithValue("@NextMaintenanceDate", lichBaoTri.NgayBaoTriTiepTheo);
            cmd.Parameters.AddWithValue("@LastMaintenanceDate", (object?)lichBaoTri.NgayBaoTriGanNhat ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Checklist", (object?)lichBaoTri.DanhSachKiemTra ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(LichBaoTri lichBaoTri)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE Schedules SET AssetID=@AssetID, MaintenanceType=@MaintenanceType,
                                             NextMaintenanceDate=@NextMaintenanceDate, LastMaintenanceDate=@LastMaintenanceDate, Checklist=@Checklist
                                             WHERE ScheduleID=@ScheduleID", conn);
            cmd.Parameters.AddWithValue("@ScheduleID", lichBaoTri.MaLichBaoTri);
            cmd.Parameters.AddWithValue("@AssetID", lichBaoTri.MaTaiSan);
            cmd.Parameters.AddWithValue("@MaintenanceType", lichBaoTri.LoaiBaoTri);
            cmd.Parameters.AddWithValue("@NextMaintenanceDate", lichBaoTri.NgayBaoTriTiepTheo);
            cmd.Parameters.AddWithValue("@LastMaintenanceDate", (object?)lichBaoTri.NgayBaoTriGanNhat ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Checklist", (object?)lichBaoTri.DanhSachKiemTra ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maLichBaoTri)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM Schedules WHERE ScheduleID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maLichBaoTri);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}