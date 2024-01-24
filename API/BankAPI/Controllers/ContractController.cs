using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("Bank/[controller]/[action]")]
    public class ContractController : Controller
    {
        public ContractController(ILogger<ContractController> logger)
        {
            _logger = logger;
        }
        private ILogger<ContractController> _logger;

        [HttpGet]
        public async Task<Contract?> Get(Guid idContract)
        {
            return await new ContractService().Get(idContract);
        }
        [HttpPost]
        public async Task Add(Contract contract)
        {
            await new ContractService().Add(contract);
        }
        [HttpPut]
        public async Task Update(Contract contract)
        {
            await new ContractService().Update(contract);
        }
        [HttpDelete]
        public async Task Delete(Guid idContract)
        {
            await new ContractService().Delete(idContract);
        }
    }
}
