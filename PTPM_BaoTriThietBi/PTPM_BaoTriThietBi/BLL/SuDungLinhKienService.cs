using Model;
using DAL.Interfaces;
using BLL.Interfaces;

namespace BLL
{
    public class SuDungLinhKienService : ISuDungLinhKienService
    {
        private readonly ISuDungLinhKienRepository _repo;
        public SuDungLinhKienService(ISuDungLinhKienRepository repo) => _repo = repo;

        public IEnumerable<SuDungLinhKien> LayTatCa() => _repo.LayTatCa();
        public SuDungLinhKien? LayTheoMa(int maSuDungLinhKien) => _repo.LayTheoMa(maSuDungLinhKien);
        public void Them(SuDungLinhKien suDungLinhKien) => _repo.Them(suDungLinhKien);
        public void Sua(SuDungLinhKien suDungLinhKien) => _repo.Sua(suDungLinhKien);
        public void Xoa(int maSuDungLinhKien) => _repo.Xoa(maSuDungLinhKien);
    }
}