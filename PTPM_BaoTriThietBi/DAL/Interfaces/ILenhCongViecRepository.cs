using Model;

namespace DAL.Interfaces
{
    public interface ILenhCongViecRepository
    {
        IEnumerable<LenhCongViec> LayTatCa();
        LenhCongViec? LayTheoMa(int maLenhCongViec);
        void Them(LenhCongViec lenhCongViec);
        void Sua(LenhCongViec lenhCongViec);
        void Xoa(int maLenhCongViec);
    }
}