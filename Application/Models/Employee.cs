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
            Passport passport,
            string numberPhone,
            JobPosition jobPosition,
            Currency salary,
            DateOnly startWorkDate,
            DateOnly endContractDate) 
            : base(passport, numberPhone) 
        {
            JobPositionType = jobPosition;
            Salary = salary;
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
                client.Passport!,
                client.NumberPhone,
                JobPosition.Trainee,
                new Currency(0, CurrencyType.Dollar),
                new DateOnly(2024, 6, 5),
                new DateOnly(2025, 6, 5));
        }
        public void UpdateSalary(Currency currency) => Salary = currency;
        public DateOnly GetStartDateWork => _startWorkDate;
        public DateOnly GetEndContractDate => _endContractDate;
        public override string GetInformation()
        {
            return $"Имя: {Name}\n" +
                $"Возраст: {Age}\n" +
                $"Номер телефона: {NumberPhone}\n" +
                $"Должность: {JobPositionType}\n" +
                $"Заработная плата: {Salary.Value} {Salary.TypeCurrency}\n" +
                $"Приступил к работе: {_startWorkDate}\n" +
                $"Контракт заканчивается через " +
                $"{Convert.ToDateTime(_endContractDate.ToString()).Subtract(DateTime.Today).TotalDays} дней\n";
        }
        public override int GetHashCode()
        {
            return NumberPhone.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Employee) 
                return false;
            var employee = (Employee)obj;
            return 
                employee.Name == Name &&
                employee.Age == employee.Age &&
                employee.NumberPhone == employee.NumberPhone;
        }
    }
}
