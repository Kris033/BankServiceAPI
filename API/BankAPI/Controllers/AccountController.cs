using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("Bank/[controller]/[action]")]
    public class AccountController : Controller
    {
        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        private ILogger<AccountController> _logger;
        [HttpGet("{id}")]
        public async Task<IEnumerable<Account>> GetClientAccounts(Guid idClient)
        {
            return await new ClientService().GetAccounts(idClient);
        }
        [HttpGet("{id}")]
        public async Task<Account?> Get(Guid idAccount)
        {
            return await new ClientService().GetAccount(idAccount);
        }
        [HttpPut]
        public async Task Update(Account account)
        {
            await new ClientService().ChangeAccountClient(account);
        }
        [HttpPost]
        public async Task Add(Account account)
        {
            await new ClientService().AddAccount(account);
        }
        [HttpDelete("{id}")]
        public async Task Delete(Guid idAccount)
        {
            await new ClientService().DeleteAccountClient(idAccount);
        }
    }
}
