

using Models.Validations;

namespace Models
{
    public class Person
    {
        public Person(Passport? passport, string numberPhone)
        {
            Passport = passport;
            NumberPhone = numberPhone;
        }
        private Passport? _passport;
        public Passport? Passport
        {
            get { return _passport; }
            private set
            {
                _passport = value;
                if(_passport != null)
                {
                    Name = _passport.GetFullName();
                    Age = _passport.GetAge();
                }
                if (_passport == value) return;
            }
        }
        public string Name { get; protected set; } = string.Empty;
        public int Age { get; protected set; }
        public string NumberPhone { get; protected set; }
        public void ChangeNumberPhone(string number)
        {
            this.ValidationFieldPhoneNumber(number);
            NumberPhone = number;
        }
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
