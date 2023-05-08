using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public class UserService : IUserService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IFavoritesService _favoritesService;
        protected readonly IRecordService _recordService;

        public UserService(ApplicationDbContext context, IFavoritesService favoritesService, IRecordService recordService)
        {
            _context = context;
            _favoritesService = favoritesService;
            _recordService = recordService;
        }

        public async Task<User> GetUserByEmail(string? email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("Email is NULL");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new ArgumentNullException($"User with Email: {email} not found.");
            }

            return user;
        }

        public async Task<User> GetUserById(string? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException($"User with ID: {userId} not found.");
            }

            return user;
        }

        public async Task<UserViewModel> GetUserViewModelById(string? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException($"User with ID: {userId} not found.");
            }

            return await ToViewModel(user);
        }

        public async Task<UserViewModel> ToViewModel(User user)
        {
            return new UserViewModel()
            {
                UserId = user.Id,
                Name = user.Name,
                SurName = user.SurName,
                DateOfBirth = user.DateOfBirth,
                PhotoPath = user.PhotoPath,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FavoriteBusinesses = await _favoritesService.GetFavoriteBusinesses(user.Id),
                FavoriteServices = await _favoritesService.GetFavoriteServices(user.Id),
                Records = await _recordService.GetRecordsByUserId(user.Id)
            };
        }
    }
}
