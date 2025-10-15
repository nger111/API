using Model;

namespace BLL.Interfaces
{
    public interface ITaiSanService
    {
        IEnumerable<TaiSan> LayTatCa();
        TaiSan? LayTheoMa(int maTaiSan);
        void Them(TaiSan taiSan);
        void Sua(TaiSan taiSan);
        void Xoa(int maTaiSan);
    }
}