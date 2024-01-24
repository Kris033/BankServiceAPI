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
        [HttpGet]
        public async Task<Passport?> Get(Guid idPassport)
            => await new PassportService().Get(idPassport);
        
        [HttpPost]
        public async Task Add(Passport passport)
            => await new PassportService().Add(passport);
        
        [HttpPut]
        public async Task Update(Passport passport)
            => await new PassportService().Update(passport);
        
        [HttpDelete]
        public async Task Delete(Guid idPassport) 
            => await new PassportService().Delete(idPassport);
        
    }
}
