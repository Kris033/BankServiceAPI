using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractController : Controller
    {
        public ContractController(ILogger<ContractController> logger)
        {
            _logger = logger;
        }
        private ILogger<ContractController> _logger;
        //[HttpGet(Name = "GetPassports")]
        //public async Task<IEnumerable<Passport>> GetPassports()
        //{
        //    var passports = await new PassportService().GetPassports();
        //    return passports.AsEnumerable();
        //}
        [HttpGet(Name = "GetContract")]
        public async Task<Contract?> GetContract(Guid idContract)
        {
            return await new ContractService().GetContract(idContract);
        }
        [HttpPut(Name = "PutContract")]
        public async Task PutContract(Contract contract)
        {
            await new ContractService().AddContract(contract);
        }
        [HttpPost(Name = "PostContract")]
        public async Task PostContract(Contract contract)
        {
            await new ContractService().AddContract(contract);
        }
        [HttpDelete(Name = "DeleteContract")]
        public async Task DeleteContract(Guid idContract)
        {
            await new ContractService().DeleteContract(idContract);
        }
    }
}
