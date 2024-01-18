using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Requests;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        public ClientController(ILogger<ClientController> logger) 
        {
            _logger = logger;
        }
        private ILogger<ClientController> _logger;
        //[HttpGet(Name = "GetClients")]
        //public async Task<IEnumerable<Client>> GetClients()
        //{
        //    var clients = await new ClientService().GetClients();
        //    return clients.AsEnumerable();
        //}
        [HttpGet(Name = "GetClient")]
        public async Task<Client?> GetClient(Guid idClient)
        {
            return await new ClientService().GetClient(idClient);
        }
        [HttpPut(Name = "PutClient")]
        public async Task PutClient(Client client)
        {
            await new ClientService().AddClient(client);
        }
        [HttpPost(Name = "PostClient")]
        public async Task PostClient(Client client)
        {
            await new ClientService().UpdateClient(client);
        }
        [HttpDelete(Name = "DeleteClient")]
        public async Task DeleteClient(Guid idClient)
        {
            await new ClientService().DeleteClient(idClient);
        }
    }
}
