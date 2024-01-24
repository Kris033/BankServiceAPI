using ExportTool;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Exports;
using Models.Requests;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("Bank/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }
        private ILogger<EmployeeController> _logger;
        
        [HttpGet]
        public async Task<EmployeeExportModel?> Get(Guid idEmployee)
            => await new ExportEmployeeService().ConvertatorToExportModel(
                await new EmployeeService().Get(idEmployee));
        
        [HttpPost]
        public async Task Add(Employee employee) 
            => await new EmployeeService().Add(employee);
        
        [HttpPut]
        public async Task Update(Employee employee) 
            => await new EmployeeService().Update(employee);
        
        [HttpDelete]
        public async Task Delete(Guid idEmployee) 
            => await new EmployeeService().Delete(idEmployee);
        
    }
}
