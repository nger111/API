using Model;

namespace BLL.Interfaces
{
    public interface IVeSuCoService
    {
        IEnumerable<VeSuCo> LayTatCa();
        VeSuCo? LayTheoMa(int maVeSuCo);
        void Them(VeSuCo veSuCo);
        void Sua(VeSuCo veSuCo);
        void Xoa(int maVeSuCo);
    }
}