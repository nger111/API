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
    public class WarrantyController : ControllerBase
    {
        private readonly IWarrantiesBusiness _warrantyBusiness;
        private readonly IMemoryCache _cache;

        public WarrantyController(IWarrantiesBusiness warrantyBusiness, IMemoryCache cache)
        {
            _warrantyBusiness = warrantyBusiness;
            _cache = cache;
        }

        // Chỉ Admin tạo warranty
        [Authorize(Roles = "Admin")]
        [HttpPost("create-warranties")]
        public Warranties Create([FromBody] Warranties model)
        {
            _warrantyBusiness.Create(model);
            _cache.Remove("all-warranties");
            return model;
        }

        // Tất cả role xem warranty theo ID
        [HttpGet("get-by-id/{id}")]
        public Warranties GetById(string id) => _warrantyBusiness.GetDatabyID(id);

        // Tất cả role xem danh sách warranties
        [HttpGet("get-all")]
        public IEnumerable<Warranties> GetAll()
        {
            if (!_cache.TryGetValue("all-warranties", out List<Warranties>? list))
            {
                list = _warrantyBusiness.GetDataAll();
                _cache.Set("all-warranties", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        // Chỉ Admin update warranty
        [Authorize(Roles = "Admin")]
        [HttpPut("update-warranty/{id:int}")]
        public IActionResult Update(int id, [FromBody] Warranties model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.WarrantyID = id;

            var ok = _warrantyBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-warranties");
            return Ok(model);
        }

        // Chỉ Admin xóa
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-warranty/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            var ok = _warrantyBusiness.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-warranties");
            return NoContent();
        }
    }
}