using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        private ILogger<AccountController> _logger;
        //[HttpGet(Name = "GetPassports")]
        //public async Task<IEnumerable<Passport>> GetPassports()
        //{
        //    var passports = await new PassportService().GetPassports();
        //    return passports.AsEnumerable();
        //}
        [HttpGet(Name = "GetAccount")]
        public async Task<Account?> GetAccount(Guid idAccount)
        {
            return await new ClientService().GetAccount(idAccount);
        }
        [HttpPut(Name = "PutAccount")]
        public async Task PutAccount(Account account)
        {
            await new ClientService().AddAccount(account);
        }
        [HttpPost(Name = "PostAccount")]
        public async Task PostAccount(Account account)
        {
            await new ClientService().ChangeAccountClient(account);
        }
        [HttpDelete(Name = "DeleteAccount")]
        public async Task DeleteAccount(Guid idAccount)
        {
            await new ClientService().DeleteAccountClient(idAccount);
        }
    }
}
