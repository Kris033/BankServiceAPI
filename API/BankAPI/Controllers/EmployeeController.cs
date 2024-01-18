using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }
        private ILogger<EmployeeController> _logger;
        //[HttpGet(Name = "GetEmployees")]
        //public async Task<IEnumerable<Employee>> GetEmployees()
        //{
        //    var employees = await new EmployeeService().GetEmployees();
        //    return employees.AsEnumerable();
        //}
        [HttpGet(Name = "GetEmployee")]
        public async Task<Employee?> GetEmployee(Guid idEmployee)
        {
            return await new EmployeeService().GetEmployee(idEmployee);
        }
        [HttpPut(Name = "PutEmployee")]
        public async Task PutEmployee(Employee employee)
        {
            await new EmployeeService().AddEmployee(employee);
        }
        [HttpPost(Name = "PostEmployee")]
        public async Task PostEmployee(Employee employee)
        {
            await new EmployeeService().ChangeEmployee(employee);
        }
        [HttpDelete(Name = "DeleteEmployee")]
        public async Task DeleteEmployee(Guid idEmployee)
        {
            await new EmployeeService().DeleteEmployee(idEmployee);
        }
    }
}
