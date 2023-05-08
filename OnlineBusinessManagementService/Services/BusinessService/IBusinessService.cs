namespace OnlineBusinessManagementService.Services
{
    public interface IBusinessService
    {
        Task<BusinessViewModel> GetBusiness(int? businessId);
        Task<BusinessViewModel> GetBusinessByUserId(string? userId);
        Task<List<BusinessViewModel>> GetBusinessesByCategoryId(int? categoryId);
        Task<List<BusinessViewModel>> GetBusinesses();
        Task<Business> UpdateBusiness(BusinessViewModel model);
        Task<Business> CreateBusiness(BusinessViewModel model);
        Task<bool> DeleteBusiness(int? id);
        Task<BusinessViewModel> ToViewModel(Business business);
        Task<bool> AddWorker(User user, int? businessId);
        Task<bool> RemoveWorker(int? workerId);
    }
}
