using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;

namespace OnlineBusinessManagementService.Services
{
    public class CategoryService : ICategoryService
    {
        protected readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryOfService> CreateCategory(CategoryOfService category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<CategoryOfService> UpdateCategory(CategoryOfService category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                throw new ArgumentNullException();
            }

            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryOfService>> GetCategories()
        {
            return (await _context.Categories.OrderBy(c => c.Name).ToListAsync());
        }

        public async Task<CategoryOfService> GetCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                throw new ArgumentNullException();
            }

            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            return category;
        }
    }
}
