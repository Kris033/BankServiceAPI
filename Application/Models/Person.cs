namespace Models
{
    public class Person
    {
        public Person(string name, int age, string numberPhone)
        {
            Name = name;
            Age = age;
            NumberPhone = numberPhone;
        }
        public string Name { get; private set; } = string.Empty;
        public int Age { get; private set; }
        public string NumberPhone { get; private set; }
        public virtual string GetInformation()
        {
            return $"Имя: {Name}\n" + 
                $"Возраст: {Age}\n" +
                $"Номер телефона: {NumberPhone}\n";
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
