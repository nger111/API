using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace PTPM_BaoTriThietBi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeSuCoController : ControllerBase
    {
        private readonly IVeSuCoService _svc;
        public VeSuCoController(IVeSuCoService svc) => _svc = svc;

        [HttpPost("create")]
        public VeSuCo Create(VeSuCo model)
        {
            model.TrangThai = string.IsNullOrEmpty(model.TrangThai) ? "Open" : model.TrangThai;
            _svc.Them(model);
            return model;
        }

        [HttpPost("update")]
        public VeSuCo Update(VeSuCo model)
        {
            _svc.Sua(model);
            return model;
        }

        // Hủy = đóng vé để không vi phạm CHECK constraint và giữ lịch sử
        [HttpPost("cancel/{id:int}")]
        public IActionResult Cancel(int id)
        {
            var cur = _svc.LayTheoMa(id);
            if (cur is null) return NotFound();
            cur.TrangThai = "Closed";
            _svc.Sua(cur);
            return NoContent();
        }

        // Nếu bạn thật sự muốn xóa khỏi DB thì giữ endpoint này,
        // còn nếu đã có cancel ở trên, có thể bỏ để tránh nhầm lẫn.
        [HttpDelete("{id:int}")]
        public void Delete(int id) => _svc.Xoa(id);

        [HttpGet("get-all")]
        public IEnumerable<VeSuCo> GetAll() => _svc.LayTatCa();

        [HttpGet("get-by-id/{id:int}")]
        public VeSuCo? GetById(int id) => _svc.LayTheoMa(id);
    }
}