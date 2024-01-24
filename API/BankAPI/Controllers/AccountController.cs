using ExportTool;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Exports;
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
        [HttpGet]
        public async Task<AccountExportModel?> Get(Guid idAccount)
        {
            return await new ExportAccountsService()
                .ConvertatorToExportModel(await new AccountService().Get(idAccount));
        }
        [HttpPost]
        public async Task Add(Account account)
        {
            await new AccountService().Add(account);
        }
        [HttpPut]
        public async Task Update(Account account)
        {
            await new AccountService().Update(account);
        }
        [HttpDelete]
        public async Task Delete(Guid idAccount)
        {
            await new AccountService().Delete(idAccount);
        }
    }
}
