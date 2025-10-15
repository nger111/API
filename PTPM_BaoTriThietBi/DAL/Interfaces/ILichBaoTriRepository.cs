using Model;

namespace DAL.Interfaces
{
    public interface ILichBaoTriRepository
    {
        IEnumerable<LichBaoTri> LayTatCa();
        LichBaoTri? LayTheoMa(int maLichBaoTri);
        void Them(LichBaoTri lichBaoTri);
        void Sua(LichBaoTri lichBaoTri);
        void Xoa(int maLichBaoTri);
    }
}