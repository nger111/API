using Model;

namespace DAL.Interfaces
{
    public interface ISuDungLinhKienRepository
    {
        IEnumerable<SuDungLinhKien> LayTatCa();
        SuDungLinhKien? LayTheoMa(int maSuDungLinhKien);
        void Them(SuDungLinhKien suDungLinhKien);
        void Sua(SuDungLinhKien suDungLinhKien);
        void Xoa(int maSuDungLinhKien);
    }
}