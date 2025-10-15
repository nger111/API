using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class NguoiDungService : INguoiDungService
    {
        private readonly INguoiDungRepository _repo;
        public NguoiDungService(INguoiDungRepository repo) => _repo = repo;

        public IEnumerable<NguoiDung> LayTatCa() => _repo.LayTatCa();
        public NguoiDung? LayTheoMa(int maNguoiDung) => _repo.LayTheoMa(maNguoiDung);
        public void Them(NguoiDung nguoiDung) => _repo.Them(nguoiDung);
        public void Sua(NguoiDung nguoiDung) => _repo.Sua(nguoiDung);
        public void Xoa(int maNguoiDung) => _repo.Xoa(maNguoiDung);
    }
}