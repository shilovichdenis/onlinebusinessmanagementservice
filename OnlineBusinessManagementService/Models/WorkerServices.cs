namespace OnlineBusinessManagementService.Models
{
    public class WorkerServices
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
