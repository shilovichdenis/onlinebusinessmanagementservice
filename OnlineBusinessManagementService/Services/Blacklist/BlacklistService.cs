using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;

namespace OnlineBusinessManagementService.Services
{
    public class BlacklistService : IBlacklistService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IClientService _clientService;
        public BlacklistService(ApplicationDbContext context, IClientService clientService)
        {
            _context = context;
            _clientService = clientService;
        }

        public async Task<bool> AddClientToList(int? clientId, int? businessId)
        {
            if (clientId == null || businessId == null)
            {
                throw new ArgumentNullException();
            }
            if (!await _context.Blacklist.AnyAsync(b => b.ClientId == clientId && b.BusinessId == businessId))
            {
                await _context.Blacklist.AddAsync(new Blacklist() { BusinessId = (int)businessId, ClientId = clientId });
                await _context.SaveChangesAsync();
            }

            var client = await _clientService.GetClient(clientId);

            if (client == null)
            {
                throw new ArgumentNullException();
            }

            client.inBlacklist = true;
            await _clientService.UpdateClient(client);
            return true;
        }

        public async Task<bool> RemoveClientFromList(int? clientId, int? businessId)
        {
            if (clientId == null || businessId == null)
            {
                throw new ArgumentNullException();
            }

            var blacklist = await _context.Blacklist.FirstOrDefaultAsync(b => b.BusinessId == businessId && b.ClientId == clientId);
            if (blacklist == null)
            {
                throw new ArgumentNullException();
            }

            _context.Blacklist.Remove(blacklist);
            await _context.SaveChangesAsync();

            var client = await _clientService.GetClient(clientId);

            if (client == null)
            {
                throw new ArgumentNullException();
            }

            client.inBlacklist = false;
            await _clientService.UpdateClient(client);
            return true;
        }
    }
}
