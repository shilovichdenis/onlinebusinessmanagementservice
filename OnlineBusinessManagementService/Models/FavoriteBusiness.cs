namespace OnlineBusinessManagementService.Models
{
    public class FavoriteBusiness
    {
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        public Business? Business { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
