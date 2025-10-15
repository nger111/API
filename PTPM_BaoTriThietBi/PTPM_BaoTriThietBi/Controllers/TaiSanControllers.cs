using Microsoft.AspNetCore.Mvc;
using Model;
using BLL.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiSanController : ControllerBase
    {
        private readonly ITaiSanService _taiSanService;

        public TaiSanController(ITaiSanService taiSanService)
        {
            _taiSanService = taiSanService;
        }

        // GET: api/TaiSan
        [HttpGet]
        public ActionResult<IEnumerable<TaiSan>> GetAll()
        {
            var ds = _taiSanService.LayTatCa();
            return Ok(ds);
        }

        // GET: api/TaiSan/{id}
        [HttpGet("{id:int}")]
        public ActionResult<TaiSan> GetById(int id)
        {
            var taiSan = _taiSanService.LayTheoMa(id);
            if (taiSan == null) return NotFound();
            return Ok(taiSan);
        }

        // GET: api/TaiSan/timkiem?ten=xxx
        [HttpGet("timkiem")]
        public ActionResult<IEnumerable<TaiSan>> TimKiemTheoTen([FromQuery] string ten)
        {
            var ds = _taiSanService.LayTatCa()
                                   .Where(t => t.TenTaiSan.Contains(ten, StringComparison.OrdinalIgnoreCase));
            return Ok(ds);
        }

        // POST: api/TaiSan
        [HttpPost]
        public IActionResult Them([FromBody] TaiSan taiSan)
        {
            if (taiSan == null) return BadRequest();
            _taiSanService.Them(taiSan);
            return CreatedAtAction(nameof(GetById), new { id = taiSan.MaTaiSan }, taiSan);
        }

        // PUT: api/TaiSan/{id}
        [HttpPut("{id:int}")]
        public IActionResult Sua(int id, [FromBody] TaiSan taiSan)
        {
            if (taiSan == null || taiSan.MaTaiSan != id) return BadRequest();

            var existing = _taiSanService.LayTheoMa(id);
            if (existing == null) return NotFound();

            _taiSanService.Sua(taiSan);
            return NoContent();
        }

        // DELETE: api/TaiSan/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Xoa(int id)
        {
            var existing = _taiSanService.LayTheoMa(id);
            if (existing == null) return NotFound();

            _taiSanService.Xoa(id);
            return NoContent();
        }
    }
}
