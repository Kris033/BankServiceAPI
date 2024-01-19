using Microsoft.AspNetCore.Mvc;
using Models;
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
        public async Task<IEnumerable<Employee>> GetEmployees([FromQuery] GetFilterRequest? filterRequest = null)
        {
            return await new EmployeeService().GetEmployees(filterRequest);
        }
        [HttpGet("{id}")]
        public async Task<Employee?> Get(Guid idEmployee)
        {
            return await new EmployeeService().GetEmployee(idEmployee);
        }
        [HttpPost]
        public async Task Add(Employee employee)
        {
            await new EmployeeService().ChangeEmployee(employee);
        }
        [HttpPut]
        public async Task Update(Employee employee)
        {
            await new EmployeeService().AddEmployee(employee);
        }
        [HttpDelete("{id}")]
        public async Task Delete(Guid idEmployee)
        {
            await new EmployeeService().DeleteEmployee(idEmployee);
        }
    }
}
