using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BLL;
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
    public class TicketController : ControllerBase
    {
        private readonly ITicketBusiness _ticketBusiness;
        private readonly IMemoryCache _cache;

        public TicketController(ITicketBusiness ticketBusiness, IMemoryCache cache)
        {
            _ticketBusiness = ticketBusiness;
            _cache = cache;
        }

        // Tất cả role có thể tạo ticket
        [HttpPost("create-tickets")]
        public Tickets CreateTicket([FromBody] Tickets model)
        {
            _ticketBusiness.Create(model);
            _cache.Remove("all-tickets");
            return model;
        }

        [HttpGet("get-by-id/{id}")]
        public Tickets GetDatabyID(string id) => _ticketBusiness.GetDatabyID(id);

        // Admin và Technician xem tất cả
        [Authorize(Roles = "Admin,Technician")]
        [HttpGet("get-all")]
        public IEnumerable<Tickets> GetDataAll()
        {
            if (!_cache.TryGetValue("all-tickets", out List<Tickets>? list))
            {
                list = _ticketBusiness.GetDataAll();
                _cache.Set("all-tickets", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        // Staff chỉ xem ticket của mình
        [HttpGet("get-my-tickets")]
        public IEnumerable<Tickets> GetMyTickets()
        {
            var userId = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
            var allTickets = _ticketBusiness.GetDataAll();
            
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "Admin" || userRole == "Technician")
                return allTickets; // Admin/Technician thấy tất cả
            
            // Staff chỉ thấy ticket mình tạo hoặc được assigned
            return allTickets.Where(t => t.CreatedBy == userId || t.AssignedTo == userId).ToList();
        }

        // Admin và Technician (được assigned) mới update
        [Authorize(Roles = "Admin,Technician")]
        [HttpPut("update-ticket/{id:int}")]
        public IActionResult UpdateTicket(int id, [FromBody] Tickets model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst("UserID")?.Value ?? "0");

            // Nếu là Technician, kiểm tra có được giao ticket này không
            if (userRole == "Technician")
            {
                var ticket = _ticketBusiness.GetDatabyID(id.ToString());
                if (ticket.AssignedTo != userId)
                    return Forbid("Bạn chỉ có thể cập nhật ticket được giao cho mình.");
            }

            model.TicketID = id;
            var ok = _ticketBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-tickets");
            return Ok(model);
        }

        // Chỉ Admin xóa
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-ticket/{id:int}")]
        public IActionResult DeleteTicket(int id)
        {
            if (id <= 0) return BadRequest("Id không hợp lệ.");
            var ok = _ticketBusiness.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-tickets");
            return NoContent();
        }
    }
}
