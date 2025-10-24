using System;
using System.Collections.Generic;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;

namespace API.Controllers
{
    [Route("api/[controller]")] // => /api/Parts
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

        [HttpPost("create")]
        public Parts Create([FromBody] Parts model)
        {
            _partsBusiness.Create(model);
            _cache.Remove("all-parts");
            return model;
        }

        [HttpGet("get-by-id/{id}")]
        public Parts GetById(string id) => _partsBusiness.GetDatabyID(id);

        [HttpGet("get-all")]
        public IEnumerable<Parts> GetAll()
        {
            if (!_cache.TryGetValue("all-parts", out List<Parts>? list))
            {
                list = _partsBusiness.GetDataAll();
                _cache.Set("all-parts", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        [HttpPut("update/{id:int}")]
        public IActionResult Update(int id, [FromBody] Parts model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.PartID = id;

            var ok = _partsBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-parts");
            return Ok(model);
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Id không h?p l?.");

            var ok = _partsBusiness.Delete(id);
            if (!ok) return NotFound();

            _cache.Remove("all-parts");
            return NoContent();
        }
    }
}