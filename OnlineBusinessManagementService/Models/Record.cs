namespace OnlineBusinessManagementService.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int? WorkerId { get; set; }
        public Worker? Worker { get; set; }
        public int? TimeScheduleId { get; set; }
        public Schedule? TimeSchedule { get; set; }
        public int TotalPrice { get; set; }
        public bool isConfirmed { get; set; }
        public bool isSuccessful { get; set; }
    }
}
