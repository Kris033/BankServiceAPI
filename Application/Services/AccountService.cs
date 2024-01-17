using BankDbConnection;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Validations;

namespace Services
{
    public class AccountService
    {
        public string GetBalance(Currency currency) => $"Баланс: {currency.Value} {currency.TypeCurrency}.";
        public async Task Put(Guid idAccount, Currency currency)
        {
            using var db = new BankContext();
            var account = await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
            var currencyAccount = await db.Currency.FirstOrDefaultAsync(c => c.Id == account!.CurrencyIdAmount);
            if (currency.TypeCurrency != currencyAccount!.TypeCurrency)
                currency.ExChange(currencyAccount!.TypeCurrency);
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
            using var db = new BankContext();
            var account = await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
            var currencyAccount = await db.Currency.FirstOrDefaultAsync(c => c.Id == account!.CurrencyIdAmount);
            if (currency.TypeCurrency != currencyAccount!.TypeCurrency)
                currency.ExChange(currencyAccount.TypeCurrency);
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
    }
}
