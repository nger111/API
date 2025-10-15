using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class LenhCongViecService : ILenhCongViecService
    {
        private readonly ILenhCongViecRepository _repo;
        public LenhCongViecService(ILenhCongViecRepository repo) => _repo = repo;

        public IEnumerable<LenhCongViec> LayTatCa() => _repo.LayTatCa();
        public LenhCongViec? LayTheoMa(int maLenhCongViec) => _repo.LayTheoMa(maLenhCongViec);
        public void Them(LenhCongViec lenhCongViec) => _repo.Them(lenhCongViec);
        public void Sua(LenhCongViec lenhCongViec) => _repo.Sua(lenhCongViec);
        public void Xoa(int maLenhCongViec) => _repo.Xoa(maLenhCongViec);
    }
}