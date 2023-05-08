using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace OnlineBusinessManagementService.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ClientController : Controller
    {
        protected readonly IClientService _clientService;
        protected readonly IBlacklistService _blacklistService;
        public ClientController(IClientService clientService, IBlacklistService blacklistService)
        {
            _clientService = clientService;
            _blacklistService = blacklistService;
        }

        public async Task<IActionResult> _List(int? businessId)
        {
            var clients = await _clientService.GetClientsByBusinessId(businessId);
            return View(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int clientId, int sale, int businessId)
        {
            try
            {
                var client = await _clientService.GetClient(clientId);
                client.Sale = sale;
                await _clientService.UpdateClient(client);
                return RedirectToAction("_List", "Client", new { area = "Manager", businessId = businessId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int clientId, int businessId)
        {
            try
            {
                await _clientService.DeleteClient(clientId);
                return RedirectToAction("_List", "Client", new { area = "Manager", businessId = businessId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToBlacklist(int clientId, int businessId)
        {
            try
            {
                await _blacklistService.AddClientToList(clientId, businessId);
                return RedirectToAction("_List", "Client", new { area = "Manager", businessId = businessId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromBlacklist(int clientId, int businessId)
        {
            try
            {
                await _blacklistService.RemoveClientFromList(clientId, businessId);
                return RedirectToAction("_List", "Client", new { area = "Manager", businessId = businessId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
