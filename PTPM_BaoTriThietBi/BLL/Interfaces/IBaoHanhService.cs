using Model;

namespace BLL.Interfaces
{
    public interface IBaoHanhService
    {
        IEnumerable<BaoHanh> LayTatCa();
        BaoHanh? LayTheoMa(int maBaoHanh);
        void Them(BaoHanh baoHanh);
        void Sua(BaoHanh baoHanh);
        void Xoa(int maBaoHanh);
    }
}