using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class WorkOrderController : ControllerBase
    {
        private readonly IWorkOrdersBusiness _business;
        private readonly IMemoryCache _cache;

        public WorkOrderController(IWorkOrdersBusiness business, IMemoryCache cache)
        {
            _business = business;
            _cache = cache;
        }

        // Chỉ Admin tạo WorkOrder
        [Authorize(Roles = "Admin")]
        [HttpPost("create-workorder")]
        public WorkOrders Create([FromBody] WorkOrders model)
        {
            _business.Create(model);
            _cache.Remove("all-workorders");
            return model;
        }

        [HttpGet("get-by-id/{id}")]
        public WorkOrders GetById(string id) => _business.GetDatabyID(id);

        // Admin xem tất cả
        [Authorize(Roles = "Admin")]
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

        // Technician xem WorkOrder được giao cho mình
        [Authorize(Roles = "Technician")]
        [HttpGet("get-my-workorders")]
        public IEnumerable<WorkOrders> GetMyWorkOrders()
        {
            var userId = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
            var allWorkOrders = _business.GetDataAll();
            return allWorkOrders.Where(wo => wo.AssignedTo == userId).ToList();
        }

        // Admin update bất kỳ, Technician chỉ update của mình
        [Authorize(Roles = "Admin,Technician")]
        [HttpPut("update-workorder/{id:int}")]
        public IActionResult Update(int id, [FromBody] WorkOrders model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst("UserID")?.Value ?? "0");

            // Technician chỉ update WorkOrder được giao cho mình
            if (userRole == "Technician")
            {
                var workOrder = _business.GetDatabyID(id.ToString());
                if (workOrder.AssignedTo != userId)
                    return Forbid("Bạn chỉ có thể cập nhật WorkOrder được giao cho mình.");
            }

            model.WorkOrderID = id;
            var ok = _business.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-workorders");
            return Ok(model);
        }

        // Chỉ Admin xóa
        [Authorize(Roles = "Admin")]
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