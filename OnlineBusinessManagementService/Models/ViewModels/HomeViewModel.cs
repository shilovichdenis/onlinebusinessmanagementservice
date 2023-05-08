namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<AdvertisementViewModel>? Advertisements { get; set; }
        public List<BusinessViewModel>? Businesses { get; set; }
        public List<CategoryOfService>? Categories { get; set; }
    }
}
