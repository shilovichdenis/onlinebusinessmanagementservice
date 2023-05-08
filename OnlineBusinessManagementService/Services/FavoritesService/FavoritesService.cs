using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;

namespace OnlineBusinessManagementService.Services
{
    public class FavoritesService : IFavoritesService
    {
        protected readonly ApplicationDbContext _context;
        public FavoritesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFavorite(string? userId, int? id, Type type)
        {
            if (userId == null || id == null)
            {
                throw new ArgumentNullException();
            }

            if (type == typeof(BusinessViewModel))
            {
                await _context.FavoriteBusinesses.AddAsync(new FavoriteBusiness() { BusinessId = id, UserId = userId });
                await _context.SaveChangesAsync();
                return true;
            }

            if (type == typeof(ServiceViewModel))
            {
                await _context.FavoriteServices.AddAsync(new FavoriteService() { ServiceId = id, UserId = userId });
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<FavoriteBusiness>?> GetFavoriteBusinesses(string? userId)
        {
            var favoriteBusinesses = await _context.FavoriteBusinesses
                .Include(f => f.Business).Include(f => f.User).Where(f => f.UserId == userId).ToListAsync();
            return favoriteBusinesses;
        }

        public async Task<List<FavoriteService>?> GetFavoriteServices(string? userId)
        {
            var favoriteServices = await _context.FavoriteServices
                .Include(f => f.Service).Include(f => f.User).Where(f => f.UserId == userId).ToListAsync();
            return favoriteServices;
        }

        public async Task<bool> RemoveFavorite(string? userId, int? id, Type type)
        {
            if (userId == null || type == null || id == null)
            {
                throw new ArgumentNullException();
            }

            if (type == typeof(BusinessViewModel))
            {
                var favorite = await _context.FavoriteBusinesses.FirstOrDefaultAsync(f => f.UserId == userId && f.BusinessId == id);
                if (favorite == null)
                {
                    throw new ArgumentNullException();
                }

                _context.FavoriteBusinesses.Remove(favorite);
                await _context.SaveChangesAsync();
                return true;
            }

            if (type == typeof(ServiceViewModel))
            {
                var favorite = await _context.FavoriteServices.FirstOrDefaultAsync(f => f.UserId == userId && f.ServiceId == id);
                if (favorite == null)
                {
                    throw new ArgumentNullException();
                }

                _context.FavoriteServices.Remove(favorite);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
