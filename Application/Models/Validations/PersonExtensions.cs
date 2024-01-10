using Models.Exceptions;

namespace Models.Validations
{
    public static partial class PersonExtensions
    {
        public static void ValidationPerson(this Person person)
        {
            if (person.Passport == null)
                throw new ArgumentNullException("Отсутствуют паспортные данные");
            if (person.Age < 18)
                throw new Under18Exception("Человек младше 18 лет");
            person.ValidationFieldPhoneNumber(person.NumberPhone);
        }
        public static void ValidationFieldPhoneNumber(this Person person, string numberPhone)
        {
            string[] numberThreePart = numberPhone.Split('-');
            if (numberThreePart.Length != 3)
                throw new ArgumentOutOfRangeException("Номер телефона не поделен на 3 части и/или не поделен знаком \"-\"");
            if (numberThreePart[0].Length != 3 &&
                numberThreePart[1].Length != 4 &&
                numberThreePart[2].Length != 3)
                throw new ArgumentOutOfRangeException("Номер телефона не соответствует размерам");
            if (numberThreePart.Any(pn => pn.Any(n => !Char.IsDigit(n))))
                throw new ArgumentException("В номере телефона содержатся недопустимые символы");
        }
    }
}
