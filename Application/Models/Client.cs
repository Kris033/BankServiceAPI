namespace Models
{
    public class Client : Person
    {
        public Client(string name, int age, string numberPhone) : base(name)
        {
            Age = age;
            NumberPhone = numberPhone;
        }
        public int Age { get; private set; }
        public string NumberPhone { get; private set; }
        public Account Account { get; set; }
        public string GetClient()
            => $"Клиент: {Name}\n" +
               $"Возраст: {Age}\n" +
               $"Телефон: {NumberPhone}";
    }
}
