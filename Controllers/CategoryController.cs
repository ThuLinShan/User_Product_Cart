using Microsoft.AspNetCore.Mvc;
using User_Product_Cart.Dtos.Category;
using User_Product_Cart.Interface;

namespace User_Product_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController: Controller
    {
        private readonly ICategory _categoryRepository;
        public CategoryController(ICategory categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _categoryRepository.GetCategoryById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {
            var response = await _categoryRepository.GetCategoryList();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetMainCategories")]
        public async Task<IActionResult> GetMainCategories()
        {
            var response = await _categoryRepository.GetMainCategory();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCategorizedProducts")]
        public async Task<IActionResult> GetCategorizedProducts()
        {
            var response = await _categoryRepository.GetCategorizedProducts();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest req)
        {
            var response = await _categoryRepository.CreateCategory(req);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddProductToCategory")]
        public async Task<IActionResult> AddProductToCategory(int productId, int categoryId)
        {
            var response = await _categoryRepository.AddProductToCategory(productId, categoryId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest req)
        {
            var response = await _categoryRepository.UpdateCategory(id, req);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("RemoveCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryRepository.DeleteCategory(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
