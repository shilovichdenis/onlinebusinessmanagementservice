namespace OnlineBusinessManagementService.Models
{
    public class BusinessReview: Review
    {
        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
