using Model;

namespace DAL.Interfaces
{
    public interface IBaoHanhRepository
    {
        IEnumerable<BaoHanh> LayTatCa();
        BaoHanh? LayTheoMa(int maBaoHanh);
        void Them(BaoHanh baoHanh);
        void Sua(BaoHanh baoHanh);
        void Xoa(int maBaoHanh);
    }
}