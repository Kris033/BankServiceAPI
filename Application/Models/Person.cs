using Models.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Person
    {
        public Person(Guid passportId, string numberPhone, string name, int age)
        {
            PassportId = passportId;
            NumberPhone = numberPhone;
            Name = name;
            Age = age;
        }
        public Guid PassportId { get; set; }
        public Passport? Passport { get; set; }
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column("age")]
        public int Age { get; set; }
        [Required]
        [Column("number_phone")]
        public string NumberPhone { get; set; }
        [Column("in_black_list")]
        public bool InBlackList { get; set; } = false;
        public void ChangeNumberPhone(string number)
        {
            this.ValidationFieldPhoneNumber(number);
            NumberPhone = number;
        }
        public override int GetHashCode()
        {
            return NumberPhone.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Person) 
                return false;
            var person = (Person)obj;
            return 
                person.Name == Name &&
                person.Age == Age &&
                person.NumberPhone == NumberPhone;
        }
    }
}
