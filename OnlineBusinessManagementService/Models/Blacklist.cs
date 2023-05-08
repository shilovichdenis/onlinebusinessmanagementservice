namespace OnlineBusinessManagementService.Models
{
    public class Blacklist
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
