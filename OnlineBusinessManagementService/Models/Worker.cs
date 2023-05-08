using Microsoft.AspNetCore.Routing.Constraints;

namespace OnlineBusinessManagementService.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int? BusinessId { get; set; }
        public Business? Business { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string ImagesPath { get; set; }
    }
}
