using BankDbConnection;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Validations;

namespace Services
{
    public class PassportService
    {
        public async Task<Passport?> GetPassport(string numberPassport)
        {
            using var db = new BankContext();
            var passport = await db.Passport.FirstOrDefaultAsync(p => p.NumberPassport.Contains(numberPassport));
            return passport;
        }
        public async Task<Passport?> GetPassport(Guid idPassport) 
        {
            using var db = new BankContext();
            return await db.Passport.FirstOrDefaultAsync(p => p.Id == idPassport);
        }
        public async Task AddPassport(Passport passport)
        {
            passport.Validation();
            using var db = new BankContext();
            await db.Passport.AddAsync(passport);
            await db.SaveChangesAsync();
            
        }
        public async Task DeletePassport(Guid id)
        {
            using var db = new BankContext();
            var passport = await db.Passport.FirstOrDefaultAsync(p => p.Id == id);
            if (passport != null)
            {
                db.Passport.Remove(passport);
                await db.SaveChangesAsync();
            }
        }
    }
}
