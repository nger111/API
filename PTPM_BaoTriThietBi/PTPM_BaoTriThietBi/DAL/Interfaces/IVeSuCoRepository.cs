using Model;

namespace DAL.Interfaces
{
    public interface IVeSuCoRepository
    {
        IEnumerable<VeSuCo> LayTatCa();
        VeSuCo? LayTheoMa(int maVeSuCo);
        void Them(VeSuCo veSuCo);
        void Sua(VeSuCo veSuCo);
        void Xoa(int maVeSuCo);
    }
}