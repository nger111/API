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
    public class ScheduleController : ControllerBase
    {
        private readonly ISchedulesBusiness _business;
        private readonly IMemoryCache _cache;

        public ScheduleController(ISchedulesBusiness business, IMemoryCache cache)
        {
            _business = business;
            _cache = cache;
        }

        [HttpPost("create-schedule")]
        public Schedules Create([FromBody] Schedules model)
        {
            _business.Create(model);
            _cache.Remove("all-schedules");
            return model;
        }

        [HttpGet("get-by-id/{id}")]
        public Schedules GetById(string id) => _business.GetDatabyID(id);

        [HttpGet("get-all")]
        public IEnumerable<Schedules> GetAll()
        {
            if (!_cache.TryGetValue("all-schedules", out List<Schedules>? list))
            {
                list = _business.GetDataAll();
                _cache.Set("all-schedules", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        [HttpPut("update-schedule/{id:int}")]
        public IActionResult Update(int id, [FromBody] Schedules model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.ScheduleID = id;

            var ok = _business.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-schedules");
            return Ok(model);
        }

        [HttpDelete("delete-schedule/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            var ok = _business.Delete(id);
            if (!ok) return NotFound();

            _cache.Remove("all-schedules");
            return NoContent();
        }
    }
}