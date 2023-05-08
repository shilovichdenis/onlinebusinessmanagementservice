namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class RecordViewModel
    {
        public int RecordId { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; } = null;
        public int? WorkerId { get; set; } = null;
        public Worker? Worker { get; set; } = null;
        public int? TimeScheduleId { get; set; } = null;
        public Schedule? TimeSchedule { get; set; } = null;
        public int TotalPrice { get; set; } = 0;
        public bool isConfirmed { get; set; } = false;
        public bool isSuccessful { get; set; } = false;

        public List<RecordServices> Services { get; set; } = new List<RecordServices>();

        public Record ToRecord()
        {
            return new Record()
            {
                UserId = this.UserId,
                WorkerId = this.WorkerId,
                TimeScheduleId = this.TimeScheduleId,
                TotalPrice = this.TotalPrice,
                isConfirmed = this.isConfirmed,
                isSuccessful = this.isSuccessful
            };
        }

        public static void UpdateEntity(RecordViewModel model, ref Record record)
        {
            record.UserId = model.UserId;
            record.WorkerId = model.WorkerId;
            record.TimeScheduleId = model.TimeScheduleId;
            record.TotalPrice = model.TotalPrice;
            record.isConfirmed = model.isConfirmed;
            record.isSuccessful = model.isSuccessful;
        }
    }
}
