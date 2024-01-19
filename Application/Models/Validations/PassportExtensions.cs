namespace Models.Validations
{
    public static partial class PassportExtensions
    {
        public static void Validation(this Passport passport)
        {
            passport.ValidationFieldLastName();
            passport.ValidationFieldFirstName();
            passport.ValidationFieldSecondName();
            passport.ValidationFieldDateBorn();
            passport.ValidationFieldDateGivePassport();
            passport.ValidationFieldNumberPassport();
            passport.ValidationFieldPlaceBorn();
            passport.ValidationFieldLocationResidence();
        }
        public static void ValidationFieldLastName(this Passport passport)
        {
            if (passport.LastName.Length < 2 || passport.LastName.Length > 50)
                throw new ArgumentOutOfRangeException("Фамилия должна быть в диапозоне от 2-х до 50 символов");
            if (passport.LastName.Any(ln => !Char.IsLetter(ln)))
                throw new ArgumentException("В фамилии содержатся недопустимые символы");
        }
        public static void ValidationFieldFirstName(this Passport passport)
        {
            if (string.IsNullOrWhiteSpace(passport.FirstName) || passport.FirstName.Length > 40)
                throw new ArgumentOutOfRangeException("Имя должно быть в диапозоне от 1 до 40 символов");
            if (passport.FirstName.Any(fn => !Char.IsLetter(fn)))
                throw new ArgumentException("В имени содержатся недопустимые символы");
        }
        public static void ValidationFieldSecondName(this Passport passport)
        {
            if (!string.IsNullOrEmpty(passport.SecondName))
            {
                if (passport.SecondName.Length > 50)
                    throw new ArgumentOutOfRangeException("Отчество должно быть до 50 символов");
                if (passport.SecondName.Any(sn => !Char.IsLetter(sn)))
                    throw new ArgumentException("В отчестве содержаться недопустимые символы");
            }
        }
        public static void ValidationFieldDateBorn(this Passport passport)
        {
            var dateTimeToday = DateTime.Today;
            if (passport.DateBorn > dateTimeToday)
                throw new ArgumentOutOfRangeException("Человек не мог родиться в будущем");
        }
        public static void ValidationFieldDateGivePassport(this Passport passport)
        {
            if (passport.DateGivePassport < passport.DateBorn)
                throw new ArgumentOutOfRangeException("Человеку не могут дать паспорт до рождения");
        }
        public static void ValidationFieldNumberPassport(this Passport passport)
        {
            string[] numbersPasport = passport.NumberPassport.Split("-ПР №");
            if (numbersPasport.Length != 2)
                throw new ArgumentOutOfRangeException("Неправильно записана серия и номер паспорта");
            if (numbersPasport.Any(numsPart => numsPart.Any(n => !Char.IsDigit(n))))
                throw new ArgumentException("В серии и/или номере паспорта содержаться недопустимые символы");
        }
        public static void ValidationFieldPlaceBorn(this Passport passport)
        {
            if (string.IsNullOrWhiteSpace(passport.PlaceBorn) || passport.PlaceBorn.Length > 174)
                throw new ArgumentOutOfRangeException("Место рождение должно быть до 174 символов");
            string[] placeBornTwoPart = passport.PlaceBorn.Split(' ');
            if (placeBornTwoPart.Length != 2)
                throw new ArgumentOutOfRangeException("Неправильно записан формат место рождения");
            if (placeBornTwoPart[0].Length < 4 || placeBornTwoPart[0].Length > 20)
                throw new ArgumentOutOfRangeException("Диапозон типа места рождения должна быть от 4-х до 20 символов");
            if (string.IsNullOrWhiteSpace(placeBornTwoPart[1]) || placeBornTwoPart[1].Length > 168)
                throw new ArgumentOutOfRangeException("Название места рождения должно быть в диапозоне от 1 до 168 символов");
            if (placeBornTwoPart[0].Any(tp => !Char.IsLetter(tp)))
                throw new ArgumentException("Тип места рождения содержит недопустимые символы");
            if (placeBornTwoPart[1].Any(np => !Char.IsLetter(np) && np != '-'))
                throw new ArgumentException("Название места рождения содержит недопустимые символы кроме знака \"-\"");
        }
        public static void ValidationFieldLocationResidence(this Passport passport)
        {
            if (!string.IsNullOrWhiteSpace(passport.LocationResidence))
            {
                if (passport.LocationResidence.Length < 10 || passport.LocationResidence.Length > 400)
                    throw new ArgumentOutOfRangeException("Место прописки должна быть в диапозоне от 10 до 400 символов");
            }
        }
    }
}
