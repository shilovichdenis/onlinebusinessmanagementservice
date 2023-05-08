namespace OnlineBusinessManagementService.Models
{
    public class RecordServices
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public Record Record { get; set; }
        public int? ServiceId { get; set; }
        public WorkerServices? Service { get; set; }
    }
}
