namespace OnlineBusinessManagementService.Services
{
    public interface IClientService
    {
        Task<Client> GetClient(int? clientId);
        Task<List<Client>> GetClientsByBusinessId(int? businessId);
        Task<Client> CreateClient(Client model);
        Task<Client> UpdateClient(Client model);
        Task<bool> DeleteClient(int? clientId);
    }
}
