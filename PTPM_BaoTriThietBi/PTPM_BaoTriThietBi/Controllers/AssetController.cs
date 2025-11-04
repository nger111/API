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
    [Authorize] // Tất cả endpoint cần đăng nhập
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetsBusiness _assetsBusiness;
        private readonly IMemoryCache _cache;

        public AssetController(IAssetsBusiness assetsBusiness, IMemoryCache cache)
        {
            _assetsBusiness = assetsBusiness;
            _cache = cache;
        }

        // Chỉ Admin tạo asset
        [Authorize(Roles = "Admin")]
        [HttpPost("create-assets")]
        public Assets CreateAssets([FromBody] Assets model)
        {
            _assetsBusiness.Create(model);
            _cache.Remove("all-assets");
            return model;
        }

        // Tất cả role có thể xem asset theo ID
        [HttpGet("get-by-id/{id}")]
        public Assets GetDatabyID(string id) => _assetsBusiness.GetDatabyID(id);

        // Tất cả role có thể xem danh sách assets
        [HttpGet("get-all")]
        public IEnumerable<Assets> GetDataAll()
        {
            if (!_cache.TryGetValue("all-assets", out List<Assets>? list))
            {
                list = _assetsBusiness.GetDataAll();
                _cache.Set("all-assets", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        // Chỉ Admin update asset
        [Authorize(Roles = "Admin")]
        [HttpPut("update-asset/{id:int}")]
        public IActionResult UpdateAsset(int id, [FromBody] Assets model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.AssetID = id;

            var ok = _assetsBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-assets");
            return Ok(model);
        }

        // Chỉ Admin xóa (soft delete)
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-asset/{id:int}")]
        public IActionResult DeleteAsset(int id)
        {
            if (id <= 0) return BadRequest("Id không hợp lệ.");
            var ok = _assetsBusiness.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-assets");
            return NoContent();
        }
    }
}