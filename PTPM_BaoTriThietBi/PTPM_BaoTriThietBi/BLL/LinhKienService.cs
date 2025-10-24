using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class LinhKienService : ILinhKienService
    {
        private readonly ILinhKienRepository _repo;
        public LinhKienService(ILinhKienRepository repo) => _repo = repo;

        public IEnumerable<LinhKien> LayTatCa() => _repo.LayTatCa();
        public LinhKien? LayTheoMa(int maLinhKien) => _repo.LayTheoMa(maLinhKien);
        public void Them(LinhKien linhKien) => _repo.Them(linhKien);
        public void Sua(LinhKien linhKien) => _repo.Sua(linhKien);
        public void Xoa(int maLinhKien) => _repo.Xoa(maLinhKien);
    }
}