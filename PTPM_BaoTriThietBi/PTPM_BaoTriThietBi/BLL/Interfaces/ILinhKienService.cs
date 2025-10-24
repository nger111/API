using Model;

namespace BLL.Interfaces
{
    public interface ILinhKienService
    {
        IEnumerable<LinhKien> LayTatCa();
        LinhKien? LayTheoMa(int maLinhKien);
        void Them(LinhKien linhKien);
        void Sua(LinhKien linhKien);
        void Xoa(int maLinhKien);
    }
}