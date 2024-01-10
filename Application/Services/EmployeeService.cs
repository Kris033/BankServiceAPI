using Models;
using Models.Filters;
using Services.Storage;
using Models.Validations;

namespace Services
{
    public class EmployeeService
    {
        private readonly IEmployeeStorage _employees;
        public EmployeeService() 
        {
            _employees = new EmployeeStorage(
                new TestDataGenerator()
                    .GenerationEmployees(50));
        }
        public Employee GetFirstEmployee() => _employees.DataEmployees.First();
        public EmployeeStorage GetEmployees(GetFilterRequest? filterRequest = null)
        {
            var employees = _employees.DataEmployees.AsQueryable();
            if(filterRequest != null)
            {
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchFullName))
                    employees = employees
                        .Where(e => e
                            .Name
                            .Contains(filterRequest.SearchFullName));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPhone))
                    employees = employees
                        .Where(e => e
                            .NumberPhone
                            .Contains(filterRequest.SearchNumberPhone));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPassport))
                    employees = employees
                        .Where(e => e
                            .Passport!
                            .NumberPassport
                            .Contains(filterRequest.SearchNumberPassport));
                if (filterRequest.DateBornFrom != null)
                    employees = employees
                        .Where(e => e
                            .Passport!
                            .DateBorn >= filterRequest.DateBornFrom);
                if (filterRequest.DateBornTo != null)
                    employees = employees
                        .Where(e => e
                            .Passport!
                            .DateBorn <= filterRequest.DateBornTo);
            }
            return new EmployeeStorage(employees.ToList());
        }
        public Employee GetYoungestEmployee() 
            => _employees.DataEmployees
                .First(e => e
                    .Age == _employees.DataEmployees
                        .Min(ea => ea.Age));
        public Employee GetOldestEmployee()
            => _employees.DataEmployees
                .First(e => e
                    .Age == _employees.DataEmployees
                        .Max(ea => ea.Age));
        public int GetAverrageAgeEmployees() 
            => _employees.DataEmployees.Any() == false 
            ? _employees.DataEmployees.Sum(e => e.Age) / _employees.DataEmployees.Count()
            : 0;
        public void AddEmployee(Employee employee)
        {
            employee.Validation();
            _employees.Add(employee);
        }
        public void ChangeEmployee(int id, Employee employee)
        {
            if(_employees.DataEmployees.Count() < id || id < 0)
                throw new ArgumentOutOfRangeException("Такого работника по идентификатору не было найдено, т.к. вышло за пределы листа");
            employee.Validation();
            if (_employees.DataEmployees[id].Passport != employee.Passport)
                throw new ArgumentException("Работник не совпадает с изменяемым работником");
            _employees.Update(employee);
        }
    }
}
