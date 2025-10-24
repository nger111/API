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
    public class WorkOrderController : ControllerBase
    {
        private readonly IWorkOrdersBusiness _business;
        private readonly IMemoryCache _cache;

        public WorkOrderController(IWorkOrdersBusiness business, IMemoryCache cache)
        {
            _business = business;
            _cache = cache;
        }

        [HttpPost("create-workorder")]
        public WorkOrders Create([FromBody] WorkOrders model)
        {
            _business.Create(model);
            _cache.Remove("all-workorders");
            return model;
        }

        [HttpGet("get-by-id/{id}")]
        public WorkOrders GetById(string id) => _business.GetDatabyID(id);

        [HttpGet("get-all")]
        public IEnumerable<WorkOrders> GetAll()
        {
            if (!_cache.TryGetValue("all-workorders", out List<WorkOrders>? list))
            {
                list = _business.GetDataAll();
                _cache.Set("all-workorders", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        [HttpPut("update-workorder/{id:int}")]
        public IActionResult Update(int id, [FromBody] WorkOrders model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.WorkOrderID = id;

            var ok = _business.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-workorders");
            return Ok(model);
        }

        [HttpDelete("delete-workorder/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            var ok = _business.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-workorders");
            return NoContent();
        }
    }
}