namespace OnlineBusinessManagementService.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public DateTime DateTime { get; set; }
    }
}
