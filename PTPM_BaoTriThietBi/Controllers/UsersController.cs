using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    //[Authorize]
    //http://localhost:52872/api/Item/get-by-id/1
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersBusiness _usersBusiness;
        private readonly IMemoryCache _cache;

        public UsersController(IUsersBusiness usersBusiness, IMemoryCache cache)
        {
            _usersBusiness = usersBusiness;
            _cache = cache;
        }

        [HttpPost("create-users")]
        public Users Create([FromBody] Users model)
        {
            _usersBusiness.Create(model);
            _cache.Remove("all-users");
            return model;
        }

        [HttpGet("get-by-id/{id}")]
        public Users GetById(string id) => _usersBusiness.GetDatabyID(id);

        [HttpGet("get-all")]
        public IEnumerable<Users> GetAll()
        {
            if (!_cache.TryGetValue("all-users", out List<Users>? list))
            {
                list = _usersBusiness.GetDataAll();
                _cache.Set("all-users", list, TimeSpan.FromMinutes(60));
            }
            return list!;
        }

        [HttpPut("update-user/{id:int}")]
        public IActionResult Update(int id, [FromBody] Users model)
        {
            if (model == null || id <= 0) return BadRequest("Invalid payload.");
            model.UserID = id;

            var ok = _usersBusiness.Update(model);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");

            _cache.Remove("all-users");
            return Ok(model);
        }

        [HttpDelete("delete-user/{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");
            var ok = _usersBusiness.Delete(id);
            if (!ok) return NotFound();
            _cache.Remove("all-users");
            return NoContent();
        }
    }
}