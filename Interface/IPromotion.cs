using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Promotion;

namespace User_Product_Cart.Interface
{
    public interface IPromotion
    {
        Task<PromotionResponse> GetPromotion(int id);
        Task<PromotionListResponse> GetPromotions();
        Task<ResponseStatus> AddPromotion(AddPromotionRequest addPromotion);
        Task<ResponseStatus> UpdatePromotion(UpdatePromotionRequest updatePromotion);
        Task<ResponseStatus> RemovePromotion(int id);
    }
}
