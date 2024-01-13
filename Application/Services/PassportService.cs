using BankDbConnection;
using Models;
using Models.Validations;

namespace Services
{
    public class PassportService
    {
        public Passport? GetPassport(string numberPassport)
        {
            using var db = new BankContext();
            var passport = db.Passport.FirstOrDefault(p => p.NumberPassport.Contains(numberPassport));
            return passport;
        }
        public Passport? GetPassport(Guid idPassport) 
        {
            using var db = new BankContext();
            return db.Passport.FirstOrDefault(p => p.Id == idPassport);
        }
        public void AddPassport(Passport passport)
        {
            passport.Validation();
            using (var db = new BankContext())
            {
                db.Passport.Add(passport);
                db.SaveChanges();
            }
        }
        public void DeletePassport(Guid id)
        {
            using var db = new BankContext();
            var passport = db.Passport.FirstOrDefault(p => p.Id == id);
            if (passport != null)
            {
                db.Passport.Remove(passport);
                db.SaveChanges();
            }
        }
    }
}
