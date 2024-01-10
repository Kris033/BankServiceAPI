using System.ComponentModel;

namespace Models
{
    public class Account : INotifyPropertyChanged, IComparable<Account>
    {
        public Client Client { get; set; }
        public Account(Client client, string accountNumber, Currency balance)
        {
            Client = client;
            AccountNumber = accountNumber;
            BankAccount = balance;
        }
        public string AccountNumber { get; private set; }
        public Currency BankAccount { get; private set; }
        public delegate void AccountHandler (string message);
        public event AccountHandler? Notify;
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Put(Currency currency)
        {

            var response = BankAccount.Validation(currency);
            if(response.IsSuccses == false)
                OnNotify(response.ErrorMessage!);
            else
            {
                if (currency.TypeCurrency != BankAccount.TypeCurrency)
                    currency.ExChange(BankAccount.TypeCurrency);

                BankAccount.ChangeValue(BankAccount.Value + currency.Value);
                OnPropertyChanged($"Ваш счёт пополнен на {currency.Value} {currency.TypeCurrency}.");
            }
        }
        public void Remove(Currency currency)
        {
            var response = BankAccount.Validation(currency, true);
            if (response.IsSuccses == false)
                OnNotify(response.ErrorMessage!);
            else
            {
                if (currency.TypeCurrency != BankAccount.TypeCurrency)
                    currency.ExChange(BankAccount.TypeCurrency);

                BankAccount.ChangeValue(BankAccount.Value - currency.Value);
                OnPropertyChanged($"Вы сняли со счёта {currency.Value} {currency.TypeCurrency}.");
            }
        }
        public void GetBalance() => OnNotify(Balance);
        private string Balance => $"Баланс: {BankAccount.Value} {BankAccount.TypeCurrency}";
        public void OnNotify(string message) => Notify?.Invoke(message);
        public void OnPropertyChanged(string message)
        {
            PropertyChanged?.Invoke(this, 
                new PropertyChangedEventArgs(
                    message + " " + Balance));
        }
        public override int GetHashCode() => AccountNumber.GetHashCode();
        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Account) 
                return false;
            var account = (Account)obj;
            return 
                account.AccountNumber == AccountNumber &&
                account.Client.Equals(Client);
        }

        public int CompareTo(Account? account)
        {
            if (account == null) return -1;
            if (!account.Balance.Equals(Balance))
                return account.Balance.CompareTo(Balance);
            return account.AccountNumber.CompareTo(AccountNumber);
        }
    }
    public static class AccountCurrencyExtensions
    {
        public static (bool IsSuccses, string? ErrorMessage) Validation(
            this Currency accountBank,
            Currency operationCurrency,
            bool isWithDraw = false)
        {
            if (operationCurrency.Value < 0)
                return (false, 
                    isWithDraw 
                    ? "Нельзя снять со счёта отрицательное число" 
                    : "Нельзя положить на счёт отрицательное число");

            if (isWithDraw)
            {
                if (operationCurrency.Value > accountBank.Value)
                    return (false,
                        "Недостаточно средств на счету. Перепроверьте счёт и повторите попытку.");

                var resultValidation = operationCurrency.ValidationOnOperationValue();
                if (resultValidation.IsSuccses == false)
                    return resultValidation;
            }
            return (true, null);
        }
        public static (bool IsSuccses, string? ErrorMessage) ValidationOnOperationValue(this Currency operationCurrency)
            => operationCurrency.TypeCurrency switch 
            {
                CurrencyType.Euro => operationCurrency.Value > 400 ? (false, "Вы не можете снять более 400 евро за раз") : (true, null),
                CurrencyType.Dollar => operationCurrency.Value > 500 ? (false, "Вы не можете снять более 500 долларов за раз") : (true, null),
                CurrencyType.LeiMD => operationCurrency.Value > 9000 ? (false, "Вы не можете снять более 9000 леев за раз") : (true, null),
                _ => throw new InvalidDataException("Такой валюты не существует")
            };
    }
}