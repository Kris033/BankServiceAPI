using Models;
using Models.Requests;

namespace Services.Interfaces
{
    public interface IEmployeeService : IModelService<Employee>
    {
        Task<List<Employee>> GetEmployees(GetFilterRequest? filterRequest = null);
        Task<Employee> GetYoungestEmployee();
        Task<Employee> GetOldestEmployee();
        Task<int> GetAverrageAgeEmployees();
        Task<string> GetStringSalary(Guid idSalary);
        string GetInformation(Employee employee);
    }
}
