using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("account")]
    public class Account : INotifyPropertyChanged, IComparable<Account>
    {
        public Account(Guid clientId, string accountNumber, Guid currencyIdAmount)
        {
            ClientId = clientId;
            AccountNumber = accountNumber;
            CurrencyIdAmount = currencyIdAmount;
        }
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("account_number")]
        public string AccountNumber { get; private set; }
        [ForeignKey("Currency")]
        [Column("currency_id")]
        public Guid CurrencyIdAmount { get; private set; }
        public Currency? Currency { get; set; }
        [ForeignKey("Client")]
        [Column("client_id")]
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        public delegate void AccountHandler (string message);
        public event AccountHandler? Notify;
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnNotify(string message) => Notify?.Invoke(message);
        public void OnPropertyChanged(string message)
        {
            PropertyChanged?.Invoke(this, 
                new PropertyChangedEventArgs(message));
        }
        public override int GetHashCode() => AccountNumber.GetHashCode();
        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Account) 
                return false;
            var account = (Account)obj;
            return 
                account.AccountNumber == AccountNumber &&
                account.Id == Id;
        }

        public int CompareTo(Account? account)
        {
            if (account == null) return -1;
            return account.AccountNumber.CompareTo(AccountNumber);
        }
    }
}