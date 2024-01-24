using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("client")]
    public class Client : Person
    {
        public Client(string numberPhone, Guid passportId, string name, int age)
            : base(passportId, numberPhone, name, age) { }
        public List<Account> Accounts { get; set; } = new List<Account>();
        public override int GetHashCode()
        {
            return 
                Id.GetHashCode() +
                Age +
                Name.GetHashCode() +
                NumberPhone.GetHashCode() +
                InBlackList.GetHashCode() +
                PassportId.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Client) 
                return false;
            var client = (Client)obj;
            return 
                client.Id == Id &&
                client.Name == Name &&
                client.NumberPhone == NumberPhone &&
                client.Age == Age &&
                client.InBlackList == InBlackList &&
                client.PassportId == PassportId;
        }
    }
}