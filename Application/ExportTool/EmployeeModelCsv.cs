using Models;
using Models.Enums;

namespace ExportTool
{
    public class EmployeeModelCsv
    {
        public EmployeeModelCsv(
            string name,
            int age,
            string numberPhone,
            bool inBlackList,
            JobPosition jobPosition,
            Currency salary, 
            DateOnly startDateWork,
            DateOnly endDateWork,
            Guid? contractId,
            Guid passportId) 
        {
            Name = name;
            Age = age;
            NumberPhone = numberPhone;
            InBlackList = inBlackList;
            JobPosition = jobPosition;
            Salary = salary;
            StartDateWork = startDateWork;
            EndDateWork = endDateWork;
            ContractId = contractId;
            PassportId = passportId;
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public string NumberPhone { get; set; }
        public bool InBlackList { get; set; }
        public JobPosition JobPosition { get; set; }
        public Currency Salary { get; set; }
        public DateOnly StartDateWork { get; set; }
        public DateOnly EndDateWork { get; set; }
        public Guid? ContractId { get; set; }
        public Guid PassportId { get; set; }
    }
}
