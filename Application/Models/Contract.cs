using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("contract")]
    public class Contract
    {
        public Contract(
            Guid employeeId,
            string nameCompany,
            string adress,
            string city,
            string mailIndex,
            string workTime,
            string otherCondition
            )
        {
            EmployeeId = employeeId;
            NameCompany = nameCompany;
            Adress = adress;
            City = city;
            MailIndex = mailIndex;
            WorkTime = workTime;
            OtherCondition = otherCondition;
        }
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [ForeignKey("Employee")]
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        [Column("name_company")]
        public string NameCompany { get; private set; }
        [Column("adress")]
        public string Adress { get; private set; }
        [Column("city")]
        public string City { get; private set; }
        [Column("mail_index")]
        public string MailIndex { get; private set; }
        [Column("work_time")]
        public string WorkTime { get; private set; }
        [Column("other_condition")]
        public string OtherCondition { get; private set; }
        [Column("main_contract")]
        public string? MainContract { get; set; }
    }
}
