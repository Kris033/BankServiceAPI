using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("client")]
    public class Client : Person
    {
        public Client(string numberPhone, Guid passportId, string name, int age)
            : base(passportId, numberPhone, name, age) { }
        public override int GetHashCode() => NumberPhone.GetHashCode();
        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Client) 
                return false;
            var client = (Client)obj;
            return 
                client.Name == Name &&
                client.NumberPhone == NumberPhone &&
                client.Age == Age;
        }
    }
}