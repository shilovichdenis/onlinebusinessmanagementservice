namespace OnlineBusinessManagementService.Models
{
    public class Business
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkingHours { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public CategoryOfBusiness Category { get; set; }
        public double Rating { get; set; }
        public string ImagesPath { get; set; }
        public string LogoPath { get; set; }
    }
}
