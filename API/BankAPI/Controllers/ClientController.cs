using ExportTool;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Exports;
using Models.Requests;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("Bank/[controller]/[action]")]
    public class ClientController : Controller
    {
        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
        }
        private ILogger<ClientController> _logger;
        
        [HttpGet]
        public async Task<ClientExportModel?> Get(Guid idClient)
        {
            return await new ExportClientsService()
                .ConvertatorToExportModel(await new ClientService().Get(idClient));
        }
        [HttpPost]
        public async Task Add(Client client)
        {
            await new ClientService().Add(client);
        }
        [HttpPut]
        public async Task Update(Client client)
        {
            await new ClientService().Update(client);
        }
        [HttpDelete]
        public async Task Delete(Guid idClient)
        {
            await new ClientService().Delete(idClient);
        }
    }
}
