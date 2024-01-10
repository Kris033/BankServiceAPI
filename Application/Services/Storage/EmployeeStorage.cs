using Models;
using System.Collections;

namespace Services.Storage
{
    public class EmployeeStorage : IEnumerable<Employee>, IEmployeeStorage
    {
        public List<Employee> DataEmployees { get; }
        public EmployeeStorage(List<Employee> employees) => DataEmployees = employees;
        public EmployeeStorage() => DataEmployees = new List<Employee>();
        public Employee this[int index] => DataEmployees[index];
        public IEnumerator<Employee> GetEnumerator() 
            => ((IEnumerable<Employee>)DataEmployees).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Employee employee) 
            => DataEmployees.Add(employee);

        public void Update(Employee employee) 
        {
            var foundEmployee = DataEmployees
                .First(e => e.Passport == employee.Passport);
            var index = DataEmployees.IndexOf(foundEmployee);
            DataEmployees.Insert(index, employee);
        }

        public void Delete(Employee employee)
        {
            var foundEmployee = DataEmployees
                .First(e => e.Passport == employee.Passport);
            var index = DataEmployees.IndexOf(foundEmployee);
            DataEmployees.RemoveAt(index);
        }
    }
}
