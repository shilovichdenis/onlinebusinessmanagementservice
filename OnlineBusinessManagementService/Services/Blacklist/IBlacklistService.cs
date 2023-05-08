namespace OnlineBusinessManagementService.Services
{
    public interface IBlacklistService
    {
        Task<bool> AddClientToList(int? clientId, int? businessId);
        Task<bool> RemoveClientFromList(int? clientId, int? businessId);
    }
}
