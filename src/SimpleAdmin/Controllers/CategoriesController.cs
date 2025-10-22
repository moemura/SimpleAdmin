using Microsoft.AspNetCore.Mvc;
using SimpleAdmin.Services.Categories;

namespace SimpleAdmin.Endpoints
{
    /// <summary>
    /// Controller quản lý danh mục sản phẩm
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService service) : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách tất cả danh mục sản phẩm
        /// </summary>
        /// <returns>Danh sách danh mục sản phẩm</returns>
        /// <response code="200">Trả về danh sách danh mục thành công</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var data = await service.GetAll() ?? [];
            return Ok(data);
        }
    }
} 