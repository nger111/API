using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class BaoHanhService : IBaoHanhService
    {
        private readonly IBaoHanhRepository _repo;
        public BaoHanhService(IBaoHanhRepository repo) => _repo = repo;

        public IEnumerable<BaoHanh> LayTatCa() => _repo.LayTatCa();
        public BaoHanh? LayTheoMa(int maBaoHanh) => _repo.LayTheoMa(maBaoHanh);
        public void Them(BaoHanh baoHanh) => _repo.Them(baoHanh);
        public void Sua(BaoHanh baoHanh) => _repo.Sua(baoHanh);
        public void Xoa(int maBaoHanh) => _repo.Xoa(maBaoHanh);
    }
}