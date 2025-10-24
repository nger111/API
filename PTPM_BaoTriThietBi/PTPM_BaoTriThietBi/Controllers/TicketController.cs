using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Model;

namespace API.Controllers
{
    //[Authorize]
    //http://localhost:52872/api/Item/get-by-id/1
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private ITicketBusiness _ticketBusiness;
        private IMemoryCache _memoryCache;
        public TicketController(ITicketBusiness ticketBusiness, IMemoryCache memoryCache)
        {
            _ticketBusiness = ticketBusiness;
            _memoryCache = memoryCache;
        }

        [Route("create-tickets")]
        [HttpPost]
        public Tickets CreateTickets([FromBody] Tickets model)
        {
            //_memoryCache.Remove("all-item");
            _ticketBusiness.Create(model);
            return model;
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public Tickets GetDatabyID(string id)
        {
            return _ticketBusiness.GetDatabyID(id);
        }

        [Route("get-all")]
        [HttpGet]
        public IEnumerable<Tickets> GetDatabAll()
        {
            var list = _memoryCache.Get<List<Tickets>>("all-item");
            if (list == null)
            {
                var result = _ticketBusiness.GetDataAll();
                _memoryCache.Set("all-item", result, TimeSpan.FromMinutes(60));
                return result;
            }
            else
            {
                return list;
            }
        }

        // NEW: Update ticket
        [Route("update-ticket/{id:int}")]
        [HttpPut]
        public IActionResult UpdateTicket(int id, [FromBody] Tickets model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.TicketID = id;

            var ok = _ticketBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _memoryCache.Remove("all-item");
            return Ok(model);
        }

        // NEW: Delete ticket
        [Route("delete-ticket/{id:int}")]
        [HttpDelete]
        public IActionResult DeleteTicket(int id)
        {
            if (id <= 0) return BadRequest("Id không hợp lệ.");

            var ok = _ticketBusiness.Delete(id);
            if (!ok) return NotFound();

            _memoryCache.Remove("all-item");
            return NoContent();
        }

    }
}
