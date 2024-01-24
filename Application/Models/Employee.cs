using Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("employee")]
    public class Employee : Person
    {
        public Employee(
            Guid passportId,
            string numberPhone,
            string name,
            int age,
            JobPosition jobPositionType,
            Guid currencyIdSalary,
            DateTime startWorkDate,
            DateTime endContractDate) 
            : base(passportId, numberPhone, name, age) 
        {
            JobPositionType = jobPositionType;
            CurrencyIdSalary = currencyIdSalary;
            StartWorkDate = startWorkDate;
            EndContractDate = endContractDate;
        }
        public Contract? Contract { get; set; }
        [Required]
        [Column("start_work_date")]
        public DateTime StartWorkDate { get; set; }
        [Required]
        [Column("end_contract_date")]
        public DateTime EndContractDate { get; set; }
        [Required]
        [Column("job_position_type")]
        public JobPosition JobPositionType { get; set; }
        [ForeignKey("Currency")]
        [Column("currency_id")]
        public Guid CurrencyIdSalary { get; set; }
        public Currency? Currency { get; set; }
        public override int GetHashCode()
        {
            return 
                Id.GetHashCode() +
                Age + Name.GetHashCode() +
                NumberPhone.GetHashCode() + 
                InBlackList.GetHashCode() +
                PassportId.GetHashCode() +
                JobPositionType.GetHashCode() +
                CurrencyIdSalary.GetHashCode() +
                StartWorkDate.GetHashCode() +
                EndContractDate.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Employee) 
                return false;
            var employee = (Employee)obj;
            return 
                employee.Id == Id &&
                employee.Name == Name &&
                employee.Age == employee.Age &&
                employee.NumberPhone == employee.NumberPhone &&
                employee.InBlackList == employee.InBlackList &&
                employee.PassportId == employee.PassportId &&
                employee.CurrencyIdSalary == employee.CurrencyIdSalary &&
                employee.StartWorkDate == StartWorkDate &&
                employee.EndContractDate == EndContractDate;
        }
    }
}
