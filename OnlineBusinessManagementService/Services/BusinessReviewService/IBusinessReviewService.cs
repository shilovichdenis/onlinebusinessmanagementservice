namespace OnlineBusinessManagementService.Services
{
    public interface IBusinessReviewService
    {
        Task<BusinessReviewViewModel> GetBusinessReview(int? id);
        Task<List<BusinessReviewViewModel>> GetBusinessReviewsByBusinessId(int? businessId);
        Task<List<BusinessReviewViewModel>> GetBusinessReviewsByUserId(string? userId);
        Task<List<BusinessReviewViewModel>> GetBusinessReviews();
        Task<BusinessReview> CreateBusinessReview(BusinessReviewViewModel model);
        Task<BusinessReview> UpdateBusinessReview(BusinessReviewViewModel model);
        Task<bool> DeleteBusinessReview(int? id);
        BusinessReviewViewModel ToViewModel(BusinessReview businessReview);
        List<BusinessReviewViewModel> ToViewModels(List<BusinessReview> businessReviews);
    }
}
