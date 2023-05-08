namespace OnlineBusinessManagementService.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(string? userId);
        Task<User> GetUserByEmail(string? email);
        Task<UserViewModel> GetUserViewModelById(string? userId);
        Task<UserViewModel> ToViewModel(User user);
    }
}
