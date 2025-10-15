using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class VeSuCoRepository : IVeSuCoRepository
    {
        private readonly string _chuoiKetNoi;
        public VeSuCoRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<VeSuCo> LayTatCa()
        {
            var ds = new List<VeSuCo>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Tickets", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new VeSuCo
                {
                    MaVeSuCo = rd.GetInt32(rd.GetOrdinal("TicketID")),
                    MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                    TaoBoi = rd.GetInt32(rd.GetOrdinal("CreatedBy")),
                    PhanCongCho = rd.IsDBNull(rd.GetOrdinal("AssignedTo")) ? null : rd.GetInt32(rd.GetOrdinal("AssignedTo")),
                    MucDoUuTien = rd.GetString(rd.GetOrdinal("Priority")),
                    GioSLA = rd.GetInt32(rd.GetOrdinal("SLAHours")),
                    MoTaSuCo = rd.IsDBNull(rd.GetOrdinal("IssueDescription")) ? null : rd.GetString(rd.GetOrdinal("IssueDescription")),
                    TrangThai = rd.GetString(rd.GetOrdinal("Status")),
                    NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
                });
            }
            return ds;
        }

        public VeSuCo? LayTheoMa(int maVeSuCo)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Tickets WHERE TicketID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maVeSuCo);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new VeSuCo
            {
                MaVeSuCo = rd.GetInt32(rd.GetOrdinal("TicketID")),
                MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                TaoBoi = rd.GetInt32(rd.GetOrdinal("CreatedBy")),
                PhanCongCho = rd.IsDBNull(rd.GetOrdinal("AssignedTo")) ? null : rd.GetInt32(rd.GetOrdinal("AssignedTo")),
                MucDoUuTien = rd.GetString(rd.GetOrdinal("Priority")),
                GioSLA = rd.GetInt32(rd.GetOrdinal("SLAHours")),
                MoTaSuCo = rd.IsDBNull(rd.GetOrdinal("IssueDescription")) ? null : rd.GetString(rd.GetOrdinal("IssueDescription")),
                TrangThai = rd.GetString(rd.GetOrdinal("Status")),
                NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
            };
        }

        public void Them(VeSuCo veSuCo)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO Tickets (AssetID, CreatedBy, AssignedTo, Priority, SLAHours, IssueDescription, Status)
                                             VALUES (@AssetID, @CreatedBy, @AssignedTo, @Priority, @SLAHours, @IssueDescription, @Status)", conn);
            cmd.Parameters.AddWithValue("@AssetID", veSuCo.MaTaiSan);
            cmd.Parameters.AddWithValue("@CreatedBy", veSuCo.TaoBoi);
            cmd.Parameters.AddWithValue("@AssignedTo", (object?)veSuCo.PhanCongCho ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Priority", veSuCo.MucDoUuTien);
            cmd.Parameters.AddWithValue("@SLAHours", veSuCo.GioSLA);
            cmd.Parameters.AddWithValue("@IssueDescription", (object?)veSuCo.MoTaSuCo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", veSuCo.TrangThai);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(VeSuCo veSuCo)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE Tickets SET AssetID=@AssetID, CreatedBy=@CreatedBy, AssignedTo=@AssignedTo, 
                                             Priority=@Priority, SLAHours=@SLAHours, IssueDescription=@IssueDescription, Status=@Status
                                             WHERE TicketID=@TicketID", conn);
            cmd.Parameters.AddWithValue("@TicketID", veSuCo.MaVeSuCo);
            cmd.Parameters.AddWithValue("@AssetID", veSuCo.MaTaiSan);
            cmd.Parameters.AddWithValue("@CreatedBy", veSuCo.TaoBoi);
            cmd.Parameters.AddWithValue("@AssignedTo", (object?)veSuCo.PhanCongCho ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Priority", veSuCo.MucDoUuTien);
            cmd.Parameters.AddWithValue("@SLAHours", veSuCo.GioSLA);
            cmd.Parameters.AddWithValue("@IssueDescription", (object?)veSuCo.MoTaSuCo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", veSuCo.TrangThai);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maVeSuCo)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM Tickets WHERE TicketID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maVeSuCo);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}