using Model;

namespace DAL.Interfaces
{
    public interface INguoiDungRepository
    {
        IEnumerable<NguoiDung> LayTatCa();
        NguoiDung? LayTheoMa(int maNguoiDung);
        void Them(NguoiDung nguoiDung);
        void Sua(NguoiDung nguoiDung);
        void Xoa(int maNguoiDung);
    }
}