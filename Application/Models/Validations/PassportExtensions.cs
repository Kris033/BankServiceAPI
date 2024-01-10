namespace Models.Validations
{
    public static partial class PassportExtensions
    {
        public static void Validation(this Passport passport)
        {
            if (passport.LastName.Length < 2 || passport.LastName.Length > 50)
                throw new ArgumentOutOfRangeException();
            if (passport.LastName.Any(ln => !Char.IsLetter(ln)))
                throw new ArgumentException();
            if (string.IsNullOrWhiteSpace(passport.LastName) || passport.LastName.Length > 40)
                throw new ArgumentOutOfRangeException();

            if (passport.FirstName.Any(fn => !Char.IsLetter(fn)))
                throw new ArgumentException();

            if (!string.IsNullOrEmpty(passport.SecondName))
            {
                if (passport.SecondName.Length > 50)
                    throw new ArgumentOutOfRangeException();
                if (passport.SecondName.Any(sn => !Char.IsLetter(sn)))
                    throw new ArgumentException();
            }

            var dateTimeToday = DateTime.Today;
            if (passport.DateBorn.ToDateTime(new TimeOnly()) > dateTimeToday)
                throw new ArgumentOutOfRangeException();

            if (passport.DateGivePassport < passport.DateBorn)
                throw new ArgumentOutOfRangeException();

            string[] numbersPasport = passport.NumberPassport.Split("-ПР №");
            if (numbersPasport.Length != 2)
                throw new ArgumentOutOfRangeException();
            if (numbersPasport.Any(numsPart => numsPart.Any(n => !Char.IsDigit(n))))
                throw new ArgumentException();

            if (string.IsNullOrWhiteSpace(passport.PlaceBorn) || passport.PlaceBorn.Length > 174)
                throw new ArgumentOutOfRangeException();
            string[] placeBornTwoPart = passport.PlaceBorn.Split(' ');
            if (placeBornTwoPart.Length != 2)
                throw new ArgumentOutOfRangeException();
            if (placeBornTwoPart[0].Length < 4 || placeBornTwoPart[0].Length > 20)
                throw new ArgumentOutOfRangeException();
            if (string.IsNullOrWhiteSpace(placeBornTwoPart[1]) || placeBornTwoPart[1].Length > 168)
                throw new ArgumentOutOfRangeException();
            if (placeBornTwoPart[0].Any(tp => !Char.IsLetter(tp)))
                throw new ArgumentException();
            if (placeBornTwoPart[1].Any(np => !Char.IsLetter(np) && np != '-'))
                throw new ArgumentException();
            
            if (!string.IsNullOrWhiteSpace(passport.LocationResidence))
            {
                if (passport.LocationResidence.Length < 10 || passport.LocationResidence.Length > 400)
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
