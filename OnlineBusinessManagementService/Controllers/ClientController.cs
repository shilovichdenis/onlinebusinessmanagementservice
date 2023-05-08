using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBusinessManagementService.Controllers
{
    [Authorize(Policy = "HasRoles")]
    public class ClientController : Controller
    {
        protected readonly IClientService _clientService;
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int clientId)
        {
            try
            {
                var client = await _clientService.GetClient(clientId);
                return PartialView(client);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
