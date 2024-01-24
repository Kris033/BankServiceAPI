using Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("passport")]
    public class Passport
    {
        public Passport(
            string firstName,
            string lastName,
            string? secondName,
            GenderType gender,
            DateTime dateBorn,
            string placeBorn,
            string placeGivePassport,
            DateTime dateGivePassport,
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
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("first_name")]
        public string FirstName { get; private set; } = string.Empty;
        [Required]
        [Column("last_name")]
        public string LastName { get; private set; } = string.Empty;
        [Column("second_name")]
        public string? SecondName { get; private set; }
        [Required]
        [Column("gender")]
        public GenderType Gender { get; private set; }
        [Required]
        [Column("date_born")]
        public DateTime DateBorn { get; private set; }
        [Required]
        [Column("place_born")]
        public string PlaceBorn { get; private set; }
        [Required]
        [Column("place_give_passport")]
        public string PlaceGivePassport { get; private set; }
        [Required]
        [Column("number_passport")]
        public string NumberPassport { get; private set; }
        [Required]
        [Column("date_give_passport")]
        public DateTime DateGivePassport { get; private set; }
        [Column("location_residence")]
        public string? LocationResidence { get; private set; }
        public Employee? Employee { get; set; }
        public Client? Client { get; set; }
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
            var dateBorn = DateBorn;
            int age = DateTime.Today.Year - dateBorn.Year;
            if (dateBorn.AddYears(age) > DateTime.Today)
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
}
