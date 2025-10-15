using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        //  Lấy tất cả tài sản
        [HttpGet]
        public ActionResult<IEnumerable<Asset>> GetAll()
        {
            var assets = _assetService.GetAll();
            return Ok(assets);
        }

        //  Tìm kiếm tài sản theo ID
        [HttpGet("{id}")]
        public ActionResult<Asset> GetById(int id)
        {
            var asset = _assetService.GetById(id);
            if (asset == null)
                return NotFound(new { message = $"Không tìm thấy tài sản ID = {id}" });
            return Ok(asset);
        }

        //  Tìm kiếm theo tên tài sản
        [HttpGet("search")]
        public ActionResult<IEnumerable<Asset>> Search([FromQuery] string keyword)
        {
            var assets = _assetService.GetAll()
                                      .Where(a => a.AssetName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                               || a.SerialNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                               || (a.Location != null && a.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase)));

            return Ok(assets);
        }

        //  Thêm tài sản
        [HttpPost]
        public IActionResult Create([FromBody] Asset asset)
        {
            if (asset == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ!" });

            asset.CreatedAt = DateTime.Now;
            _assetService.Add(asset);
            return Ok(new { message = "Thêm tài sản thành công!" });
        }

        //  Cập nhật tài sản
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Asset asset)
        {
            var existing = _assetService.GetById(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy tài sản ID = {id}" });

            asset.AssetID = id;
            _assetService.Update(asset);
            return Ok(new { message = "Cập nhật tài sản thành công!" });
        }

        //  Xóa tài sản
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _assetService.GetById(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy tài sản ID = {id}" });

            _assetService.Delete(id);
            return Ok(new { message = "Xóa tài sản thành công!" });
        }
    }
}
