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
    public class PartsController : ControllerBase
    {
        private readonly IPartsBusiness _partsBusiness;
        private readonly IMemoryCache _cache;

        public PartsController(IPartsBusiness partsBusiness, IMemoryCache cache)
        {
            _partsBusiness = partsBusiness;
            _cache = cache;
        }

        // Chỉ Admin tạo parts
        [Authorize(Roles = "Admin")]
        [HttpPost("create-parts")]
        public Parts Create([FromBody] Parts model)
        {
            _partsBusiness.Create(model);
            _cache.Remove("all-parts");
            return model;
        }

        // Admin và Technician xem parts theo ID
        [Authorize(Roles = "Admin,Technician")]
        [HttpGet("get-by-id/{id}")]
        public Parts GetDatabyID(string id) => _partsBusiness.GetDatabyID(id);

        // Admin và Technician xem danh sách parts
        [Authorize(Roles = "Admin,Technician")]
        [HttpGet("get-all")]
        public IEnumerable<Parts> GetDataAll()
        {
            if (!_cache.TryGetValue("all-parts", out List<Parts>? list))
            {
                list = _partsBusiness.GetDataAll();
                _cache.Set("all-parts", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        // Chỉ Admin update parts
        [Authorize(Roles = "Admin")]
        [HttpPut("update-parts/{id:int}")]
        public IActionResult Update(int id, [FromBody] Parts model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.PartID = id;

            var ok = _partsBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-parts");
            return Ok(model);
        }

        // Chỉ Admin xóa (soft delete)
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-parts/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            var ok = _partsBusiness.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-parts");
            return NoContent();
        }
    }
}