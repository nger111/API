using Model;

namespace BLL.Interfaces
{
    public interface ISuDungLinhKienService
    {
        IEnumerable<SuDungLinhKien> LayTatCa();
        SuDungLinhKien? LayTheoMa(int maSuDungLinhKien);
        void Them(SuDungLinhKien suDungLinhKien);
        void Sua(SuDungLinhKien suDungLinhKien);
        void Xoa(int maSuDungLinhKien);
    }
}