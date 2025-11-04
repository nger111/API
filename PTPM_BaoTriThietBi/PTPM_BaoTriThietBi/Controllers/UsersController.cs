using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Authorize] // Tất cả endpoint cần đăng nhập
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersBusiness _usersBusiness;
        private readonly IMemoryCache _cache;

        public UsersController(IUsersBusiness usersBusiness, IMemoryCache cache)
        {
            _usersBusiness = usersBusiness;
            _cache = cache;
        }

        // Chỉ Admin tạo user
        [Authorize(Roles = "Admin")]
        [HttpPost("create-users")]
        public Users Create([FromBody] Users model)
        {
            _usersBusiness.Create(model);
            _cache.Remove("all-users");
            return model;
        }

        // Admin xem tất cả, Technician/Staff xem profile của mình
        [HttpGet("get-by-id/{id}")]
        public Users GetById(string id) => _usersBusiness.GetDatabyID(id);

        // Chỉ Admin xem tất cả users
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public IEnumerable<Users> GetAll()
        {
            if (!_cache.TryGetValue("all-users", out List<Users>? list))
            {
                list = _usersBusiness.GetDataAll();
                _cache.Set("all-users", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        // Admin update bất kỳ, user khác chỉ update chính mình
        [HttpPut("update-user/{id:int}")]
        public IActionResult Update(int id, [FromBody] Users model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var currentUserId = int.Parse(User.FindFirst("UserID")?.Value ?? "0");

            // Nếu không phải Admin, chỉ được sửa chính mình
            if (userRole != "Admin" && currentUserId != id)
                return Forbid("Bạn chỉ có thể cập nhật thông tin của chính mình.");

            model.UserID = id;
            var ok = _usersBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-users");
            return Ok(model);
        }

        // Chỉ Admin xóa
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            var ok = _usersBusiness.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-users");
            return NoContent();
        }
    }
}