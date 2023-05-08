namespace OnlineBusinessManagementService.Services
{
    public interface IFavoritesService
    {
        Task<List<FavoriteBusiness>?> GetFavoriteBusinesses(string? userId);
        Task<List<FavoriteService>?> GetFavoriteServices(string? userId);
        Task<bool> AddFavorite(string? userId, int? id, Type type);
        Task<bool> RemoveFavorite(string? userId, int? id, Type type);
    }
}
