using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class LenhCongViecRepository : ILenhCongViecRepository
    {
        private readonly string _chuoiKetNoi;
        public LenhCongViecRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<LenhCongViec> LayTatCa()
        {
            var ds = new List<LenhCongViec>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM WorkOrders", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new LenhCongViec
                {
                    MaLenhCongViec = rd.GetInt32(rd.GetOrdinal("WorkOrderID")),
                    MaLichBaoTri = rd.IsDBNull(rd.GetOrdinal("ScheduleID")) ? null : rd.GetInt32(rd.GetOrdinal("ScheduleID")),
                    MaVeSuCo = rd.IsDBNull(rd.GetOrdinal("TicketID")) ? null : rd.GetInt32(rd.GetOrdinal("TicketID")),
                    MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                    PhanCongCho = rd.IsDBNull(rd.GetOrdinal("AssignedTo")) ? null : rd.GetInt32(rd.GetOrdinal("AssignedTo")),
                    LoaiCongViec = rd.GetString(rd.GetOrdinal("WorkType")),
                    MoTa = rd.IsDBNull(rd.GetOrdinal("Description")) ? null : rd.GetString(rd.GetOrdinal("Description")),
                    TrangThai = rd.GetString(rd.GetOrdinal("Status")),
                    NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt")),
                    NgayHoanThanh = rd.IsDBNull(rd.GetOrdinal("CompletedAt")) ? null : rd.GetDateTime(rd.GetOrdinal("CompletedAt"))
                });
            }
            return ds;
        }

        public LenhCongViec? LayTheoMa(int maLenhCongViec)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM WorkOrders WHERE WorkOrderID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maLenhCongViec);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new LenhCongViec
            {
                MaLenhCongViec = rd.GetInt32(rd.GetOrdinal("WorkOrderID")),
                MaLichBaoTri = rd.IsDBNull(rd.GetOrdinal("ScheduleID")) ? null : rd.GetInt32(rd.GetOrdinal("ScheduleID")),
                MaVeSuCo = rd.IsDBNull(rd.GetOrdinal("TicketID")) ? null : rd.GetInt32(rd.GetOrdinal("TicketID")),
                MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                PhanCongCho = rd.IsDBNull(rd.GetOrdinal("AssignedTo")) ? null : rd.GetInt32(rd.GetOrdinal("AssignedTo")),
                LoaiCongViec = rd.GetString(rd.GetOrdinal("WorkType")),
                MoTa = rd.IsDBNull(rd.GetOrdinal("Description")) ? null : rd.GetString(rd.GetOrdinal("Description")),
                TrangThai = rd.GetString(rd.GetOrdinal("Status")),
                NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt")),
                NgayHoanThanh = rd.IsDBNull(rd.GetOrdinal("CompletedAt")) ? null : rd.GetDateTime(rd.GetOrdinal("CompletedAt"))
            };
        }

        public void Them(LenhCongViec lenh)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO WorkOrders (ScheduleID, TicketID, AssetID, AssignedTo, WorkType, Description, Status)
                                             VALUES (@ScheduleID, @TicketID, @AssetID, @AssignedTo, @WorkType, @Description, @Status)", conn);
            cmd.Parameters.AddWithValue("@ScheduleID", (object?)lenh.MaLichBaoTri ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TicketID", (object?)lenh.MaVeSuCo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssetID", lenh.MaTaiSan);
            cmd.Parameters.AddWithValue("@AssignedTo", (object?)lenh.PhanCongCho ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkType", lenh.LoaiCongViec);
            cmd.Parameters.AddWithValue("@Description", (object?)lenh.MoTa ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", lenh.TrangThai);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(LenhCongViec lenh)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE WorkOrders SET ScheduleID=@ScheduleID, TicketID=@TicketID, AssetID=@AssetID, 
                                             AssignedTo=@AssignedTo, WorkType=@WorkType, Description=@Description, Status=@Status,
                                             CompletedAt=@CompletedAt WHERE WorkOrderID=@WorkOrderID", conn);
            cmd.Parameters.AddWithValue("@WorkOrderID", lenh.MaLenhCongViec);
            cmd.Parameters.AddWithValue("@ScheduleID", (object?)lenh.MaLichBaoTri ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TicketID", (object?)lenh.MaVeSuCo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssetID", lenh.MaTaiSan);
            cmd.Parameters.AddWithValue("@AssignedTo", (object?)lenh.PhanCongCho ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkType", lenh.LoaiCongViec);
            cmd.Parameters.AddWithValue("@Description", (object?)lenh.MoTa ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", lenh.TrangThai);
            cmd.Parameters.AddWithValue("@CompletedAt", (object?)lenh.NgayHoanThanh ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maLenhCongViec)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM WorkOrders WHERE WorkOrderID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maLenhCongViec);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}