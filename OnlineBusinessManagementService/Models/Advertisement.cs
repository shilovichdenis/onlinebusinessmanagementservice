namespace OnlineBusinessManagementService.Models
{
    public class Advertisement
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Discount { get; set; }
        public string ImagePath { get; set; }

    }
}
