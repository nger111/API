using Model;

namespace BLL.Interfaces
{
    public interface ILichBaoTriService
    {
        IEnumerable<LichBaoTri> LayTatCa();
        LichBaoTri? LayTheoMa(int maLichBaoTri);
        void Them(LichBaoTri lichBaoTri);
        void Sua(LichBaoTri lichBaoTri);
        void Xoa(int maLichBaoTri);
    }
}