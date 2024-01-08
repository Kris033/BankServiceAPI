using Models;
using Services.Validations;

namespace Services
{
    public class EmployeeService
    {
        private readonly List<Employee> _listEmployees;
        public EmployeeService() 
        {
            _listEmployees = 
                new TestDataGenerator()
                    .GenerationEmployees(50);
        }
        public Employee GetFirstEmployee() => _listEmployees.First();
        public void AddEmployee(Employee employee)
        {
            employee.Validation();
            _listEmployees.Add(employee);
        }
        public void ChangeEmployee(int id, Employee employee)
        {
            if(_listEmployees.Count < id || id < 0)
                throw new ArgumentOutOfRangeException("Такого работника по идентификатору не было найдено, т.к. вышло за пределы листа");
            employee.Validation();
            if (_listEmployees[id].Passport != employee.Passport)
                throw new ArgumentException("Работник не совпадает с изменяемым работником");
            _listEmployees.Insert(id, employee);
        }
    }
}
