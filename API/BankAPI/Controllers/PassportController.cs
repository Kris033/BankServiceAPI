using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("Bank/[controller]/[action]")]
    public class PassportController : Controller
    {
        public PassportController(ILogger<PassportController> logger)
        {
            _logger = logger;
        }
        private ILogger<PassportController> _logger;
        [HttpGet("{id}")]
        public async Task<Passport?> Get(Guid idPassport)
        {
            return await new PassportService().GetPassport(idPassport);
        }
        [HttpPost]
        public async Task Add(Passport passport)
        {
            await new PassportService().AddPassport(passport);
        }
        [HttpDelete("{id}")]
        public async Task Delete(Guid idPassport)
        {
            await new PassportService().DeletePassport(idPassport);
        }
    }
}
