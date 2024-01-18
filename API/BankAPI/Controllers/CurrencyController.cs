using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : Controller
    {
        public CurrencyController(ILogger<CurrencyController> logger)
        {
            _logger = logger;
        }
        private ILogger<CurrencyController> _logger;
        //[HttpGet(Name = "GetPassports")]
        //public async Task<IEnumerable<Passport>> GetPassports()
        //{
        //    var passports = await new PassportService().GetPassports();
        //    return passports.AsEnumerable();
        //}
        [HttpGet(Name = "GetCurrency")]
        public async Task<Currency?> GetCurrency(Guid idCurrency)
        {
            return await new CurrencyService().GetCurrency(idCurrency);
        }
        [HttpPut(Name = "PutCurrency")]
        public async Task PutCurrency(Currency currency)
        {
            await new CurrencyService().AddCurrency(currency);
        }
        [HttpPost(Name = "PostCurrency")]
        public async Task PostPassport(Currency currency)
        {
            await new CurrencyService().AddCurrency(currency);
        }
        [HttpDelete(Name = "DeleteCurrency")]
        public async Task DeleteCurrency(Guid idCurrency)
        {
            await new CurrencyService().DeleteCurrency(idCurrency);
        }
    }
}
