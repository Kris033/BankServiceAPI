using BankDbConnection;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Validations;
using Services.Interfaces;

namespace Services
{
    public class AccountService : IAccountService
    {
        public string GetBalance(Currency currency) => $"Баланс: {currency.Value} {currency.TypeCurrency}.";
        public async Task Put(Guid idAccount, Currency currency)
        {
            CurrencyService currencyService = new CurrencyService();
            using var db = new BankContext();
            var account = await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
            var currencyAccount = await db.Currency.FirstOrDefaultAsync(c => c.Id == account!.CurrencyId);
            if (currency.TypeCurrency != currencyAccount!.TypeCurrency)
                await currencyService.ExChange(currency, currencyAccount!.TypeCurrency);
            var response = currencyAccount!.Validation(currency);
            if (response.IsSuccses == false)
                account!.OnNotify(response.ErrorMessage!);
            else
            {
                currencyAccount!.ChangeValue(currencyAccount!.Value + currency.Value);
                await db.SaveChangesAsync();
                account!.OnPropertyChanged($"Ваш счёт пополнен на {currency.Value} {currency.TypeCurrency}." + GetBalance(currencyAccount));
            }
        }
        public async Task Remove(Guid idAccount, Currency currency)
        {
            CurrencyService currencyService = new CurrencyService();
            using var db = new BankContext();
            var account = await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
            var currencyAccount = await db.Currency.FirstOrDefaultAsync(c => c.Id == account!.CurrencyId);
            if (currency.TypeCurrency != currencyAccount!.TypeCurrency)
                await currencyService.ExChange(currency, currencyAccount!.TypeCurrency);
            var response = currencyAccount!.Validation(currency, true);
            if (response.IsSuccses == false)
                account!.OnNotify(response.ErrorMessage!);
            else
            {
                currencyAccount.ChangeValue(currencyAccount.Value - currency.Value);
                await db.SaveChangesAsync();
                account!.OnPropertyChanged($"Вы сняли со счёта {currency.Value} {currency.TypeCurrency}.");
            }
        }
        public async Task<Account?> Get(Guid idAccount)
        {
            using var db = new BankContext();
            return await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
        }
        public async Task<List<Account>> GetAccountsClient(Guid idClient)
        {
            using var db = new BankContext();
            return await db.Account.Where(a => a.ClientId == idClient).ToListAsync();
        }
        public async Task Add(Account account)
        {
            account.Validation();
            using var db = new BankContext();
            if (!await db.Client.AnyAsync(c => c.Id == account.ClientId))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            db.Account.Add(account);
            await db.SaveChangesAsync();
        }
        public async Task Update(Account account)
        {
            account.Validation();
            using var db = new BankContext();
            if (!await db.Client.AnyAsync(c => c.Id == account.ClientId))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            if (!await db.Account.AnyAsync(a => a.Id == account.Id))
                throw new ArgumentNullException("Такого банковского счета не существует");
            if (!await db.Account.AnyAsync(a => a.ClientId == account.ClientId))
                throw new ArgumentNullException("Такого банковского счета у клиента не существует");
            db.Account.Update(account);
            await db.SaveChangesAsync();
        }
        public async Task Delete(Guid idAccount)
        {
            using var db = new BankContext();
            var account = await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
            if (account == null)
                throw new ArgumentNullException("Удаляемый банковский счет не был найден");
            db.Account.Remove(account);
            await db.SaveChangesAsync();
        }
    }
}
