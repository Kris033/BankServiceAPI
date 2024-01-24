using BankDbConnection;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Validations;
using Services.Interfaces;

namespace Services
{
    public class PassportService : IPassportService
    {
        public async Task<Passport?> Get(Guid idPassport) 
        {
            using var db = new BankContext();
            return await db.Passport.FirstOrDefaultAsync(p => p.Id == idPassport);
        }
        public async Task Add(Passport passport)
        {
            passport.Validation();
            using var db = new BankContext();
            await db.Passport.AddAsync(passport);
            await db.SaveChangesAsync();
        }
        public async Task Update(Passport passport)
        {
            passport.Validation();
            using var db = new BankContext();
            db.Passport.Update(passport);
            await db.SaveChangesAsync();
        }
        public async Task Delete(Guid id)
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
