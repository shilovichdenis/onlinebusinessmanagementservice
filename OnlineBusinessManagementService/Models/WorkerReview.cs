namespace OnlineBusinessManagementService.Models
{
    public class WorkerReview : Review
    {
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
