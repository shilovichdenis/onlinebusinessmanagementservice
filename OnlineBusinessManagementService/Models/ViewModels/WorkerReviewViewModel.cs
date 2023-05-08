using Microsoft.EntityFrameworkCore;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class WorkerReviewViewModel
    {
        public int WorkerReviewId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public int Rating { get; set; }
        public string? Description { get; set; }


        public WorkerReview ToReview()
        {
            return new WorkerReview()
            {
                UserId = this.UserId,
                WorkerId = this.WorkerId,
                Rating = this.Rating,
                Description = this.Description,
            };
        }

        public static void UpdateEntity(WorkerReviewViewModel model, ref WorkerReview review)
        {
            review.UserId = model.UserId;
            review.WorkerId = model.WorkerId;
            review.Rating = model.Rating;
            review.Description = model.Description;
        }
    }
}
