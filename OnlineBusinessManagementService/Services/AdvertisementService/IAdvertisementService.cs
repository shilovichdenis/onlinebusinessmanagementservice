namespace OnlineBusinessManagementService.Services
{
    public interface IAdvertisementService
    {
        Task<List<AdvertisementViewModel>> GetAdvertisements();
        Task<AdvertisementViewModel> GetAdvertisement(int? advertisementId);
        Task<AdvertisementViewModel> GetAdvertisementByServiceId(int? serviceId);
        Task<List<AdvertisementViewModel>> GetAdvertisementsByBusinessId(int? businessId);
        Task<Advertisement> CreateAdvertisement(AdvertisementViewModel model);
        Task<Advertisement> UpdateAdvertisement(AdvertisementViewModel model);
        Task<bool> DeleteAdvertisement(int? advertisementId);
        Task<AdvertisementViewModel> ToViewModel(Advertisement advertisement);
        Task<List<AdvertisementViewModel>> ToViewModels(List<Advertisement> advertisements);
        Task<bool> AddService(int? advertisementId, int? serviceId);
        Task<bool> RemoveService(int? advertisementId, int? serviceId);

    }
}
