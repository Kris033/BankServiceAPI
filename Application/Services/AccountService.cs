using BankDbConnection;
using Models;
using Models.Validations;

namespace Services
{
    public class AccountService
    {
        public string Balance(Currency currency) => $"Баланс: {currency.Value} {currency.TypeCurrency}.";
        public void Put(Guid idAccount, Currency currency)
        {
            using var db = new BankContext();
            var account = db.Account.FirstOrDefault(a => a.Id == idAccount);
            var currencyAccount = db.Currency.FirstOrDefault(c => c.Id == account!.CurrencyIdAmount);
            var response = currencyAccount!.Validation(currency);
            if (response.IsSuccses == false)
                account!.OnNotify(response.ErrorMessage!);
            else
            {
                if (currency.TypeCurrency != currencyAccount!.TypeCurrency)
                    currency.ExChange(currencyAccount!.TypeCurrency);

                currencyAccount!.ChangeValue(currencyAccount!.Value + currency.Value);
                db.SaveChanges();
                account!.OnPropertyChanged($"Ваш счёт пополнен на {currency.Value} {currency.TypeCurrency}." + Balance(currencyAccount));
            }
        }
        public void Remove(Guid idAccount, Currency currency)
        {
            using var db = new BankContext();
            var account = db.Account.FirstOrDefault(a => a.Id == idAccount);
            var currencyAccount = db.Currency.FirstOrDefault(c => c.Id == account!.CurrencyIdAmount);
            if (currency.TypeCurrency != currencyAccount!.TypeCurrency)
                currency.ExChange(currencyAccount.TypeCurrency);
            var response = currencyAccount!.Validation(currency, true);
            if (response.IsSuccses == false)
                account!.OnNotify(response.ErrorMessage!);
            else
            {
                currencyAccount.ChangeValue(currencyAccount.Value - currency.Value);
                db.SaveChanges();
                account!.OnPropertyChanged($"Вы сняли со счёта {currency.Value} {currency.TypeCurrency}.");
            }
        }
    }
}
