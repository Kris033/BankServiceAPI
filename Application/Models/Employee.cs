namespace Models
{
    public enum JobPosition
    {
        Trainee,
        Cashier,
        Security,
        Director
    }
    public class Employee : Person
    {
        public Employee(
            string name,
            JobPosition jobPosition,
            DateOnly startWorkDate,
            DateOnly endContractDate) 
            : base(name) 
        {
            JobPositionType = jobPosition;
            Salary = new Currency (jobPosition switch 
            {
                JobPosition.Trainee => 200,
                JobPosition.Cashier => 900,
                JobPosition.Security => 1100,
                JobPosition.Director => 0,
                _ => throw new InvalidDataException("Такой должности не существует")
            }, CurrencyType.Dollar);
            _startWorkDate = startWorkDate;
            _endContractDate = endContractDate;
        }
        private string? _contract { get; set; }
        private DateOnly _startWorkDate { get; set; }
        private DateOnly _endContractDate { get; set; }
        public JobPosition JobPositionType { get; private set; }
        public Currency Salary { get; private set; }
        public void UpdateContract(Contract contract) 
            => _contract = contract.GetContract();

        public static explicit operator Employee(Client client)
        {
            return new Employee(
                client.Name, 
                JobPosition.Trainee,
                new DateOnly(2024, 6, 5),
                new DateOnly(2025, 6, 5));
        }
        public void UpdateSalary(Currency currency) => Salary = currency;
        public DateOnly GetStartDateWork => _startWorkDate;
        public DateOnly GetEndContractDate => _endContractDate;
        public string GetEmployeeInformation()
        {
            return $"Имя: {Name}\n" +
                $"Должность: {JobPositionType}\n" +
                $"Заработная плата: {Salary.Value} {Salary.TypeCurrency}\n" +
                $"Приступил к работе: {_startWorkDate}\n" +
                $"Контракт заканчивается через " +
                $"{Convert.ToDateTime(_endContractDate.ToString()).Subtract(DateTime.Today).TotalDays} дней\n";
        }
    }
}
