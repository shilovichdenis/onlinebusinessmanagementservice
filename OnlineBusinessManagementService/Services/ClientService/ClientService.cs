using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public class ClientService : IClientService
    {
        protected readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Client> CreateClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException();
            }

            var check = _context.Clients.Any(c => c.UserId == client.UserId && c.BusinessId == client.BusinessId);
            if (check)
            {
                return client;
            }
            else
            {
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
                return client;
            }
        }

        public async Task<Client> UpdateClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException();
            }

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> DeleteClient(int? clientId)
        {
            if (clientId == null)
            {
                throw new ArgumentNullException();
            }

            var client = await _context.Clients.FindAsync(clientId);

            if (client == null)
            {
                throw new ArgumentNullException();
            }

            var black = await _context.Blacklist.FirstOrDefaultAsync(b => b.ClientId == clientId);
            if (black != null)
            {
                _context.Blacklist.Remove(black);
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Client> GetClient(int? clientId)
        {
            if (clientId == null)
            {
                throw new ArgumentNullException();
            }

            var client = await _context.Clients.Include(c => c.User).Include(c => c.Business).FirstOrDefaultAsync(b => b.Id == clientId);

            if (client == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                return client;
            }
        }

        public async Task<List<Client>> GetClientsByBusinessId(int? businessId)
        {
            var models = new List<Client>();
            var clients = await _context.Clients.Include(c => c.User).Include(c => c.Business).Where(c => c.BusinessId == businessId).ToListAsync();

            foreach (var client in clients)
            {
                models.Add(client);
            }

            return models;
        }
    }
}
