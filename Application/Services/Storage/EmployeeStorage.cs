using Models;
using System.Collections;

namespace Services.Storage
{
    public class EmployeeStorage : IEnumerable<Employee>
    {
        private Employee[] _employees;
        public EmployeeStorage(Employee[] employees) => _employees = employees;
        public EmployeeStorage() => _employees = new Employee[0];
        public Employee this[int index] => _employees[index];
        public void Add(Employee employee)
        {
            Array.Resize(ref _employees, _employees.Length + 1);
            _employees[^1] = employee;
        }
        public void Insert(int id, Employee employee) 
            => _employees[id] = employee;
        public IEnumerator<Employee> GetEnumerator() 
            => ((IEnumerable<Employee>)_employees).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
