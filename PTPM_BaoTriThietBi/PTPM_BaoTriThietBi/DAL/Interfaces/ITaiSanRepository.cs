using Model;

namespace DAL.Interfaces
{
    public interface ITaiSanRepository
    {
        IEnumerable<TaiSan> LayTatCa();
        TaiSan? LayTheoMa(int maTaiSan);
        void Them(TaiSan taiSan);
        void Sua(TaiSan taiSan);
        void Xoa(int maTaiSan);
    }
}