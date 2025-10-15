using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class VeSuCoService : IVeSuCoService
    {
        private readonly IVeSuCoRepository _repo;
        public VeSuCoService(IVeSuCoRepository repo) => _repo = repo;

        public IEnumerable<VeSuCo> LayTatCa() => _repo.LayTatCa();
        public VeSuCo? LayTheoMa(int maVeSuCo) => _repo.LayTheoMa(maVeSuCo);
        public void Them(VeSuCo veSuCo) => _repo.Them(veSuCo);
        public void Sua(VeSuCo veSuCo) => _repo.Sua(veSuCo);
        public void Xoa(int maVeSuCo) => _repo.Xoa(maVeSuCo);
    }
}