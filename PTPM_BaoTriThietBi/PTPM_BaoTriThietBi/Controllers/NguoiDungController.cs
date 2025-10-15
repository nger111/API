using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces;
using Model;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungService _service;

        public NguoiDungController(IConfiguration configuration)
        {
            // 🔹 Lấy chuỗi kết nối từ appsettings.json
            string chuoiKetNoi = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Không tìm thấy 'DefaultConnection' trong appsettings.json");

            // 🔹 Tạo Repository và Service thủ công (không cần sửa DAL)
            INguoiDungRepository repo = new NguoiDungRepository(chuoiKetNoi);
            _service = new NguoiDungService(repo);
        }

        // ==================== LẤY TẤT CẢ ====================
        [HttpGet]
        public IActionResult LayTatCa()
        {
            try
            {
                var ds = _service.LayTatCa();
                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách người dùng thành công",
                    data = ds
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi lấy danh sách người dùng",
                    error = ex.Message
                });
            }
        }

        // ==================== LẤY THEO MÃ ====================
        [HttpGet("{id}")]
        public IActionResult LayTheoMa(int id)
        {
            try
            {
                var nd = _service.LayTheoMa(id);
                if (nd == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Không tìm thấy người dùng với mã {id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Lấy thông tin người dùng thành công",
                    data = nd
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi lấy thông tin người dùng",
                    error = ex.Message
                });
            }
        }

        // ==================== THÊM ====================
        [HttpPost]
        public IActionResult Them([FromBody] NguoiDung nguoiDung)
        {
            try
            {
                if (nguoiDung == null)
                    return BadRequest(new { success = false, message = "Dữ liệu người dùng không hợp lệ" });

                if (string.IsNullOrWhiteSpace(nguoiDung.HoTen) ||
                    string.IsNullOrWhiteSpace(nguoiDung.Email) ||
                    string.IsNullOrWhiteSpace(nguoiDung.MatKhauHash) ||
                    string.IsNullOrWhiteSpace(nguoiDung.VaiTro))
                {
                    return BadRequest(new { success = false, message = "Thiếu thông tin bắt buộc" });
                }

                var vaiTroHopLe = new[] { "Admin", "Technician", "Staff" };
                if (!vaiTroHopLe.Contains(nguoiDung.VaiTro))
                {
                    return BadRequest(new { success = false, message = "Vai trò phải là Admin, Technician hoặc Staff" });
                }

                _service.Them(nguoiDung);
                return CreatedAtAction(nameof(LayTheoMa), new { id = nguoiDung.MaNguoiDung }, new
                {
                    success = true,
                    message = "Thêm người dùng thành công",
                    data = nguoiDung
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi thêm người dùng",
                    error = ex.Message
                });
            }
        }

        // ==================== SỬA ====================
        [HttpPut("{id}")]
        public IActionResult Sua(int id, [FromBody] NguoiDung nguoiDung)
        {
            try
            {
                if (nguoiDung == null)
                    return BadRequest(new { success = false, message = "Dữ liệu người dùng không hợp lệ" });

                if (id != nguoiDung.MaNguoiDung)
                    return BadRequest(new { success = false, message = "Mã người dùng không khớp" });

                var hienTai = _service.LayTheoMa(id);
                if (hienTai == null)
                    return NotFound(new { success = false, message = $"Không tìm thấy người dùng với mã {id}" });

                _service.Sua(nguoiDung);

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật người dùng thành công",
                    data = nguoiDung
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi cập nhật người dùng",
                    error = ex.Message
                });
            }
        }

        // ==================== XÓA ====================
        [HttpDelete("{id}")]
        public IActionResult Xoa(int id)
        {
            try
            {
                var nd = _service.LayTheoMa(id);
                if (nd == null)
                    return NotFound(new { success = false, message = $"Không tìm thấy người dùng với mã {id}" });

                _service.Xoa(id);
                return Ok(new
                {
                    success = true,
                    message = "Xóa người dùng thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi xóa người dùng",
                    error = ex.Message
                });
            }
        }
    }
}
