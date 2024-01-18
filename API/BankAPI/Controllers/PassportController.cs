using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportController : Controller
    {
        public PassportController(ILogger<PassportController> logger)
        {
            _logger = logger;
        }
        private ILogger<PassportController> _logger;
        //[HttpGet(Name = "GetPassports")]
        //public async Task<IEnumerable<Passport>> GetPassports()
        //{
        //    var passports = await new PassportService().GetPassports();
        //    return passports.AsEnumerable();
        //}
        [HttpGet(Name = "GetPassport")]
        public async Task<Passport?> GetPassport(Guid idPassport)
        {
            return await new PassportService().GetPassport(idPassport);
        }
        [HttpPut(Name = "PutPassport")]
        public async Task PutPassport(Passport passport)
        {
            await new PassportService().AddPassport(passport);
        }
        [HttpDelete(Name = "DeletePassport")]
        public async Task DeletePassport(Guid idPassport)
        {
            await new PassportService().DeletePassport(idPassport);
        }
    }
}
