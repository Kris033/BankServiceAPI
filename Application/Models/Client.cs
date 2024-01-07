namespace Models
{
    public class Client : Person
    {
        public Client(string name, int age, string numberPhone) 
            : base(name, age, numberPhone) { }
        public override string GetInformation() 
            => $"Клиент: {Name}\n" + 
            $"Возраст: {Age}\n" +
            $"Телефон: {NumberPhone}\n";
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
