using Models;

namespace Services
{
    public class PersonService
    {
        public string GetInformation(Person person)
        {
            return $"Имя: {person.Name}\n" +
                $"Возраст: {person.Age}\n" +
                $"Номер телефона: {person.NumberPhone}\n";
        }
    }
}
