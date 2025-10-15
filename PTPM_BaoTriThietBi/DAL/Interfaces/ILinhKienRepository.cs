using Model;

namespace DAL.Interfaces
{
    public interface ILinhKienRepository
    {
        IEnumerable<LinhKien> LayTatCa();
        LinhKien? LayTheoMa(int maLinhKien);
        void Them(LinhKien linhKien);
        void Sua(LinhKien linhKien);
        void Xoa(int maLinhKien);
    }
}