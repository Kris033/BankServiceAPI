using Models;
using Models.Filters;
using Services.Storage;
using Services.Validations;

namespace Services
{
    public class EmployeeService
    {
        private readonly EmployeeStorage _employees;
        public EmployeeService() 
        {
            _employees = new EmployeeStorage(
                new TestDataGenerator()
                    .GenerationEmployees(50)
                    .ToArray());
        }
        public Employee GetFirstEmployee() => _employees.First();
        public EmployeeStorage GetEmployees(GetFilterRequest? filterRequest = null)
        {
            var employees = _employees.AsQueryable();
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
            return new EmployeeStorage(employees.ToArray());
        }
        public Employee GetYoungestEmployee() 
            => _employees
                .First(e => e
                    .Age == _employees
                        .Min(ea => ea.Age));
        public Employee GetOldestEmployee()
            => _employees
                .First(e => e
                    .Age == _employees
                        .Max(ea => ea.Age));
        public int GetAverrageAgeEmployees() 
            => _employees.Any() == false 
            ? _employees.Sum(e => e.Age) / _employees.Count()
            : 0;
        public void AddEmployee(Employee employee)
        {
            employee.Validation();
            _employees.Add(employee);
        }
        public void ChangeEmployee(int id, Employee employee)
        {
            if(_employees.Count() < id || id < 0)
                throw new ArgumentOutOfRangeException("Такого работника по идентификатору не было найдено, т.к. вышло за пределы листа");
            employee.Validation();
            if (_employees[id].Passport != employee.Passport)
                throw new ArgumentException("Работник не совпадает с изменяемым работником");
            _employees.Insert(id, employee);
        }
    }
}
