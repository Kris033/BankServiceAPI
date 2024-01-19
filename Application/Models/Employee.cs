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
        [Column("contract_id")]
        public Guid? ContractId { get; set; }
        [Required]
        [Column("start_work_date")]
        public DateTime StartWorkDate { get; set; }
        [Required]
        [Column("end_contract_date")]
        public DateTime EndContractDate { get; set; }
        [Required]
        [Column("job_position_type")]
        public JobPosition JobPositionType { get; private set; }
        [ForeignKey("CurrencyId")]
        [Column("currency_id")]
        public Guid CurrencyIdSalary { get; private set; }
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
