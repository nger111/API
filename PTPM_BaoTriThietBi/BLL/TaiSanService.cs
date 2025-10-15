using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class TaiSanService : ITaiSanService
    {
        private readonly ITaiSanRepository _repo;
        public TaiSanService(ITaiSanRepository repo) => _repo = repo;

        public IEnumerable<TaiSan> LayTatCa() => _repo.LayTatCa();
        public TaiSan? LayTheoMa(int maTaiSan) => _repo.LayTheoMa(maTaiSan);
        public void Them(TaiSan taiSan) => _repo.Them(taiSan);
        public void Sua(TaiSan taiSan) => _repo.Sua(taiSan);
        public void Xoa(int maTaiSan) => _repo.Xoa(maTaiSan);
    }
}