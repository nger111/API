using System;
using System.Collections.Generic;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PartUsageController : ControllerBase
    {
        private readonly IPartUsagesBusiness _business;
        private readonly IMemoryCache _cache;

        public PartUsageController(IPartUsagesBusiness business, IMemoryCache cache)
        {
            _business = business;
            _cache = cache;
        }

        // Admin và Technician tạo part usage
        [Authorize(Roles = "Admin,Technician")]
        [HttpPost("create-partusage")]
        public PartUsages Create([FromBody] PartUsages model)
        {
            _business.Create(model);
            _cache.Remove("all-partusages");
            return model;
        }

        // Admin và Technician xem part usage theo ID
        [Authorize(Roles = "Admin,Technician")]
        [HttpGet("get-by-id/{id}")]
        public PartUsages GetById(string id) => _business.GetDatabyID(id);

        // Admin và Technician xem danh sách
        [Authorize(Roles = "Admin,Technician")]
        [HttpGet("get-all")]
        public IEnumerable<PartUsages> GetAll()
        {
            if (!_cache.TryGetValue("all-partusages", out List<PartUsages>? list))
            {
                list = _business.GetDataAll();
                _cache.Set("all-partusages", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        // Admin và Technician update
        [Authorize(Roles = "Admin,Technician")]
        [HttpPut("update-partusage/{id:int}")]
        public IActionResult Update(int id, [FromBody] PartUsages model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.PartUsageID = id;

            var ok = _business.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-partusages");
            return Ok(model);
        }

        // Chỉ Admin xóa
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-partusage/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            var ok = _business.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-partusages");
            return NoContent();
        }
    }
}