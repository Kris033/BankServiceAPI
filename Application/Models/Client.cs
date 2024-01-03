namespace Models
{
    public class Client : Person
    {
        public Client(string name) : base(name) { }
        public Account Account { get; set; }
    }
}
