using BankDbConnection;
using Models.Validations;
using Models;

namespace Services
{
    public class CurrencyService
    {
        public Currency? GetCurrency(Guid id)
        {
            using var db = new BankContext();
            return db.Currency.FirstOrDefault(c => c.Id == id);
        }
        public Currency? GetCurrency(Currency currency)
        {
            using var db = new BankContext();
            return db.Currency.Find(currency.Id);
        }
        public Currency AddCurrency(Currency currency)
        {
            currency.Validation();
            using var db = new BankContext();
            db.Currency.Add(currency);
            db.SaveChanges();
            return GetCurrency(currency)!;
        }
        public void UpdateCurrency(Currency currency)
        {
            currency.Validation();
            using var db = new BankContext();
            if(!db.Currency.Any(c => c.Id == currency.Id))
                throw new ArgumentNullException("Идентификатор изменяемой валюты, не был найден");
            db.Currency.Update(currency);
            db.SaveChanges();
        }
        public void DeleteCurrency(Guid guid)
        {
            using var db = new BankContext();
            var currency = 
                db.Currency.FirstOrDefault(c => c.Id == guid) 
                ?? throw new ArgumentNullException("Идентификатор удаляемой валюты, не был найден");
            db.Currency.Remove(currency);
            db.SaveChanges();
        }
    }
}
