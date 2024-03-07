using Microsoft.AspNetCore.Mvc;
using User_Product_Cart.Dtos.Promotion;
using User_Product_Cart.Interface;

namespace User_Product_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : Controller
    {
        private readonly IPromotion _promotionRepository;
        public PromotionController(IPromotion promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        [HttpGet("GetPromotions")]
        public async Task<IActionResult> GetPromotions()
        {
            var response = await _promotionRepository.GetPromotions();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{promotionId}")]
        public async Task<IActionResult> GetPromotion(int promotionId)
        {
            var response = await _promotionRepository.GetPromotion(promotionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddPromotion([FromBody] AddPromotionRequest addPromotion)
        {
            var response = await _promotionRepository.AddPromotion(addPromotion);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{promotionId}")]
        public async Task<IActionResult> RemovePromotion(int promotionId)
        {
            var response = await _promotionRepository.RemovePromotion(promotionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePromotion([FromBody] UpdatePromotionRequest updatePromotion)
        {
            var response = await _promotionRepository.UpdatePromotion(updatePromotion);
            return StatusCode(response.StatusCode, response);
        }
    }
}
