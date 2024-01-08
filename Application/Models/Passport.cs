namespace Models
{
    public class Passport
    {
        public Passport(
            string firstName,
            string lastName,
            string? secondName,
            GenderType gender,
            DateOnly dateBorn,
            string placeBorn,
            string placeGivePassport,
            DateOnly dateGivePassport,
            string numberPassport,
            string? locationResidence) 
        {
            FirstName = firstName;
            LastName = lastName;
            SecondName = secondName;
            Gender = gender;
            DateBorn = dateBorn;
            PlaceBorn = placeBorn;
            PlaceGivePassport = placeGivePassport;
            DateGivePassport = dateGivePassport;
            NumberPassport = numberPassport;
            LocationResidence = locationResidence;
        }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string? SecondName { get; private set; }
        public GenderType Gender { get; private set; }
        public DateOnly DateBorn { get; private set; }
        public string PlaceBorn { get; private set; }
        public string PlaceGivePassport { get; private set; }
        public string NumberPassport { get; private set; }
        public DateOnly DateGivePassport { get; private set; }
        public string? LocationResidence { get; private set; }
        public string GetFullInformation()
        {
            return $"Фамилия: {LastName}\n" +
                $"Имя: {FirstName}\n" +
                $"Отчество: {SecondName ?? "отсутствует"}\n" +
                $"Пол: {Gender}\n" +
                $"Дата рождения: {DateBorn}\n" +
                $"Место рождения: {PlaceBorn}\n" +
                $"Номер и серия паспорта: {NumberPassport}\n" +
                $"Дата выдачи паспорта: {DateGivePassport}\n" +
                $"Место выдачи: {PlaceGivePassport}\n" +
                $"Прописка: {LocationResidence}\n";
        }
        public int GetAge()
        {
            var dateTimeBorn = DateBorn.ToDateTime(new TimeOnly());
            int age = DateTime.Today.Year - dateTimeBorn.Year;
            if (dateTimeBorn.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }
        public string GetFullName()
        {
            if (SecondName == null)
                return string.Join(' ', new string[] { LastName, FirstName });
            return string.Join(' ', new string[] { LastName, FirstName, SecondName });
        }
    }
    public enum GenderType
    {
        Male,
        Female
    }
}
