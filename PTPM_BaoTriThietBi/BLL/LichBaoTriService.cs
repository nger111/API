using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class LichBaoTriService : ILichBaoTriService
    {
        private readonly ILichBaoTriRepository _repo;
        public LichBaoTriService(ILichBaoTriRepository repo) => _repo = repo;

        public IEnumerable<LichBaoTri> LayTatCa() => _repo.LayTatCa();
        public LichBaoTri? LayTheoMa(int maLichBaoTri) => _repo.LayTheoMa(maLichBaoTri);
        public void Them(LichBaoTri lichBaoTri) => _repo.Them(lichBaoTri);
        public void Sua(LichBaoTri lichBaoTri) => _repo.Sua(lichBaoTri);
        public void Xoa(int maLichBaoTri) => _repo.Xoa(maLichBaoTri);
    }
}