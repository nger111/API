using System.Data.SqlClient;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class TaiSanRepository : ITaiSanRepository
    {
        private readonly string _chuoiKetNoi;
        public TaiSanRepository(string chuoiKetNoi) => _chuoiKetNoi = chuoiKetNoi;

        public IEnumerable<TaiSan> LayTatCa()
        {
            var ds = new List<TaiSan>();
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Assets", conn);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ds.Add(new TaiSan
                {
                    MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                    TenTaiSan = rd.GetString(rd.GetOrdinal("AssetName")),
                    SoSerial = rd.GetString(rd.GetOrdinal("SerialNumber")),
                    ViTri = rd.IsDBNull(rd.GetOrdinal("Location")) ? null : rd.GetString(rd.GetOrdinal("Location")),
                    NgayMua = rd.IsDBNull(rd.GetOrdinal("PurchaseDate")) ? null : rd.GetDateTime(rd.GetOrdinal("PurchaseDate")),
                    TrangThai = rd.GetString(rd.GetOrdinal("Status")),
                    NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
                });
            }
            return ds;
        }

        public TaiSan? LayTheoMa(int maTaiSan)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("SELECT * FROM Assets WHERE AssetID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maTaiSan);
            conn.Open();
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new TaiSan
            {
                MaTaiSan = rd.GetInt32(rd.GetOrdinal("AssetID")),
                TenTaiSan = rd.GetString(rd.GetOrdinal("AssetName")),
                SoSerial = rd.GetString(rd.GetOrdinal("SerialNumber")),
                ViTri = rd.IsDBNull(rd.GetOrdinal("Location")) ? null : rd.GetString(rd.GetOrdinal("Location")),
                NgayMua = rd.IsDBNull(rd.GetOrdinal("PurchaseDate")) ? null : rd.GetDateTime(rd.GetOrdinal("PurchaseDate")),
                TrangThai = rd.GetString(rd.GetOrdinal("Status")),
                NgayTao = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
            };
        }

        public void Them(TaiSan taiSan)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"INSERT INTO Assets (AssetName, SerialNumber, Location, PurchaseDate, Status)
                                             VALUES (@AssetName, @SerialNumber, @Location, @PurchaseDate, @Status)", conn);
            cmd.Parameters.AddWithValue("@AssetName", taiSan.TenTaiSan);
            cmd.Parameters.AddWithValue("@SerialNumber", taiSan.SoSerial);
            cmd.Parameters.AddWithValue("@Location", (object?)taiSan.ViTri ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PurchaseDate", (object?)taiSan.NgayMua ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", taiSan.TrangThai);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Sua(TaiSan taiSan)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand(@"UPDATE Assets SET AssetName=@AssetName, SerialNumber=@SerialNumber, Location=@Location,
                                             PurchaseDate=@PurchaseDate, Status=@Status WHERE AssetID=@AssetID", conn);
            cmd.Parameters.AddWithValue("@AssetID", taiSan.MaTaiSan);
            cmd.Parameters.AddWithValue("@AssetName", taiSan.TenTaiSan);
            cmd.Parameters.AddWithValue("@SerialNumber", taiSan.SoSerial);
            cmd.Parameters.AddWithValue("@Location", (object?)taiSan.ViTri ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PurchaseDate", (object?)taiSan.NgayMua ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", taiSan.TrangThai);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maTaiSan)
        {
            using var conn = new SqlConnection(_chuoiKetNoi);
            using var cmd = new SqlCommand("DELETE FROM Assets WHERE AssetID=@id", conn);
            cmd.Parameters.AddWithValue("@id", maTaiSan);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}