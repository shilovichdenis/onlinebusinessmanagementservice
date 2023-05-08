using Microsoft.EntityFrameworkCore;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class BusinessReviewViewModel
    {
        public int BusinessReviewId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public int Rating { get; set; }
        public string? Description { get; set; }

        public BusinessReview ToReview()
        {
            return new BusinessReview()
            {
                UserId = this.UserId,
                BusinessId = this.BusinessId,
                Rating = this.Rating,
                Description = this.Description,
            };
        }

        public static void UpdateEntity(BusinessReviewViewModel model, ref BusinessReview review)
        {
            review.UserId = model.UserId;
            review.BusinessId = model.BusinessId;
            review.Rating = model.Rating;
            review.Description = model.Description;
        }
    }
}
