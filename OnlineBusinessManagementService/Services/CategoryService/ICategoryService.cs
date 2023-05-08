namespace OnlineBusinessManagementService.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryOfService>> GetCategories();
        Task<CategoryOfService> GetCategory(int? categoryId);
        Task<CategoryOfService> CreateCategory(CategoryOfService category);
        Task<CategoryOfService> UpdateCategory(CategoryOfService category);
        Task<bool> DeleteCategory(int? categoryId);
    }
}
