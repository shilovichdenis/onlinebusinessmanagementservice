namespace OnlineBusinessManagementService.Models
{
    public class FavoriteService
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
