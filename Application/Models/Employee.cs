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
            Currency salary,
            DateOnly startWorkDate,
            DateOnly endContractDate) 
            : base(name) 
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
        {
            string contractString = contract.GetContract();
            _contract = contractString;
            Console.WriteLine(contractString);
        }
        public DateOnly GetStartDateWork => _startWorkDate;
        public DateOnly GetEndContractDate => _endContractDate;
    }
}
