using Microsoft.AspNetCore.Mvc;
using Models;
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
        public async Task<IEnumerable<Client>> GetClients([FromQuery] GetFilterRequest? filterRequest = null)
        {
            return await new ClientService().GetClients(filterRequest);
        }
        [HttpGet("{id}")]
        public async Task<Client?> Get(Guid idClient)
        {
            return await new ClientService().GetClient(idClient);
        }
        [HttpPut]
        public async Task Update(Client client)
        {
            await new ClientService().UpdateClient(client);
        }
        [HttpPost]
        public async Task Add(Client client)
        {
            await new ClientService().AddClient(client);
        }
        [HttpDelete("{id}")]
        public async Task Delete(Guid idClient)
        {
            await new ClientService().DeleteClient(idClient);
        }
    }
}
