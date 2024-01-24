using Models.Enums;

namespace Models.Exports
{
    public class EmployeeExportModel
    {
        public EmployeeExportModel(
            string name,
            int age,
            string numberPhone,
            bool inBlackList,
            JobPosition jobPosition,
            Currency salary, 
            DateTime startDateWork,
            DateTime endDateWork,
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
            PassportId = passportId;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string NumberPhone { get; set; }
        public bool InBlackList { get; set; }
        public JobPosition JobPosition { get; set; }
        public Currency Salary { get; set; }
        public DateTime StartDateWork { get; set; }
        public DateTime EndDateWork { get; set; }
        public Guid PassportId { get; set; }
    }
}
