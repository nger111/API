using System;
using System.Collections.Generic;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;

namespace API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetsBusiness _assetsBusiness;
            private readonly IMemoryCache _memoryCache;

        public AssetController(IAssetsBusiness assetsBusiness, IMemoryCache memoryCache)
        {
            _assetsBusiness = assetsBusiness;
            _memoryCache = memoryCache;
        }

        [Route("create-assets")]
        [HttpPost]
        public Assets CreateAssets([FromBody] Assets model)
        {
            _assetsBusiness.Create(model);
            _memoryCache.Remove("all-assets");
            return model;
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public Assets GetDatabyID(string id)
        {
            return _assetsBusiness.GetDatabyID(id);
        }

        [Route("get-all")]
        [HttpGet]
        public IEnumerable<Assets> GetDataAll()
        {
            var list = _memoryCache.Get<List<Assets>>("all-assets");
            if (list == null)
            {
                var result = _assetsBusiness.GetDataAll();
                _memoryCache.Set("all-assets", result, TimeSpan.FromMinutes(60));
                return result;
            }
            else
            {
                return list;
            }
        }

        [Route("update-asset/{id:int}")]
        [HttpPut]
        public IActionResult UpdateAsset(int id, [FromBody] Assets model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.AssetID = id;

            var ok = _assetsBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _memoryCache.Remove("all-assets");
            return Ok(model);
        }

        [Route("delete-asset/{id:int}")]
        [HttpDelete]
        public IActionResult DeleteAsset(int id)
        {
            if (id <= 0) return BadRequest("Id không hợp lệ.");

            var ok = _assetsBusiness.Delete(id);
            if (!ok) return NotFound();

            _memoryCache.Remove("all-assets");
            return NoContent();
        }
    }
}