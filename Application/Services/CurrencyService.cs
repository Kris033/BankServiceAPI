using BankDbConnection;
using Models.Validations;
using Models;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Response;
using Newtonsoft.Json;

namespace Services
{
    public class CurrencyService
    {
        public async Task ExChange(Currency currency, CurrencyType currencyTypeExChange)
        {
            using var httpClient = new HttpClient();
            HttpResponseMessage responseMessage =
                await httpClient.GetAsync($"https://www.amdoren.com/api/currency.php?api_key=SJXtXrBuTgSX9WwGPgkEXPWq9rXUMw&from={currency.TypeCurrency}&to={currencyTypeExChange}&amount={currency.Value}");
            var message = await responseMessage.Content.ReadAsStringAsync();
            var exChangeResponse = JsonConvert.DeserializeObject<GetExChangeResponse>(message);

            if (exChangeResponse!.Error < 0)
            {
                currency.ChangeValue(exChangeResponse.Amount);
                currency.TypeCurrency = currencyTypeExChange;
            }
        }
        public async Task<Currency?> GetCurrency(Guid id)
        {
            using var db = new BankContext();
            return await db.Currency.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Currency?> GetCurrency(Currency currency)
        {
            using var db = new BankContext();
            return await db.Currency.FindAsync(currency.Id);
        }
        public async Task AddCurrency(Currency currency)
        {
            currency.Validation();
            using var db = new BankContext();
            db.Currency.Add(currency);
            await db.SaveChangesAsync();
        }
        public async Task UpdateCurrency(Currency currency)
        {
            currency.Validation();
            using var db = new BankContext();
            if(!await db.Currency.AnyAsync(c => c.Id == currency.Id))
                throw new ArgumentNullException("Идентификатор изменяемой валюты, не был найден");
            db.Currency.Update(currency);
            await db.SaveChangesAsync();
        }
        public async Task DeleteCurrency(Guid guid)
        {
            using var db = new BankContext();
            var currency = 
                await db.Currency.FirstOrDefaultAsync(c => c.Id == guid) 
                ?? throw new ArgumentNullException("Идентификатор удаляемой валюты, не был найден");
            db.Currency.Remove(currency);
            await db.SaveChangesAsync();
        }
    }
}
