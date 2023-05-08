namespace OnlineBusinessManagementService.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public int Sale { get; set; }
        public bool inBlacklist { get; set; } = false;
    }
}
