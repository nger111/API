using Model;

namespace BLL.Interfaces
{
    public interface ILenhCongViecService
    {
        IEnumerable<LenhCongViec> LayTatCa();
        LenhCongViec? LayTheoMa(int maLenhCongViec);
        void Them(LenhCongViec lenhCongViec);
        void Sua(LenhCongViec lenhCongViec);
        void Xoa(int maLenhCongViec);
    }
}