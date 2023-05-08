namespace OnlineBusinessManagementService.Models
{
    public class AdvertisementServices
    {
        public int Id { get; set; }
        public Advertisement Advertisement { get; set; }
        public int AdvertisementId { get; set; }
        public Service? Service { get; set; }
        public int? ServiceId { get; set; }
    }
}
