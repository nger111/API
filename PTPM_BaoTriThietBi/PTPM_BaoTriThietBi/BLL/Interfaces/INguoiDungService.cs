using Model;

namespace BLL.Interfaces
{
    public interface INguoiDungService
    {
        IEnumerable<NguoiDung> LayTatCa();
        NguoiDung? LayTheoMa(int maNguoiDung);
        void Them(NguoiDung nguoiDung);
        void Sua(NguoiDung nguoiDung);
        void Xoa(int maNguoiDung);
    }
}