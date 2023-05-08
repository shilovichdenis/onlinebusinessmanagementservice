namespace OnlineBusinessManagementService.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public int CategoryId { get; set; }
        public CategoryOfService Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Time { get; set; }
        public string ImagePath { get; set; }
    }
}
