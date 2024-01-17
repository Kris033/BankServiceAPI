using Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Models
{
    [Table("currency")]
    public class Currency : IComparable<Currency>
    {
        public static decimal LeiToDollarExchangeRate { get; private set; } = 0.0058M;
        public static decimal DollarToLeiExchangeRate { get; private set; } = 17.30M;
        public static decimal LeiToEuroExchangeRate { get; private set; } = 0.0052M;
        public static decimal EuroToLeiExchangeRate { get; private set; } = 19.16M;
        public static decimal EuroToDollarExchangeRate { get; private set; } = 1.11M;
        public static decimal DollarToEuroExchangeRate { get; private set; } = 0.90M;

        public Currency(decimal value, CurrencyType typeCurrency) 
        {
            Value = value;
            TypeCurrency = typeCurrency;
        }
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("value")]
        public decimal Value { get; private set; }
        [Column("type_currency")]
        public CurrencyType TypeCurrency { get; private set; }
        public void ExChange(CurrencyType exTypeChange)
        {
            Value = Math.Round(exTypeChange switch
            {
                CurrencyType.Euro => TypeCurrency switch
                {
                    CurrencyType.Dollar => Value * DollarToEuroExchangeRate,
                    CurrencyType.LeiMD => Value * LeiToEuroExchangeRate,
                    _ => Value
                },
                CurrencyType.Dollar => TypeCurrency switch
                {
                    CurrencyType.Euro => Value * EuroToDollarExchangeRate,
                    CurrencyType.LeiMD => Value * LeiToDollarExchangeRate,
                    _ => Value
                },
               CurrencyType.LeiMD => TypeCurrency switch
               {
                   CurrencyType.Dollar => Value * DollarToLeiExchangeRate,
                   CurrencyType.Euro => Value * EuroToLeiExchangeRate,
                   _ => Value
               },
                _ => Value
            }, 2);
            TypeCurrency = exTypeChange;
        }
        public void ChangeValue(decimal valueNow)
            => Value = Math.Round(valueNow, 2);
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null || obj is not Currency)
                return false;
            var currency = (Currency)obj;
            return 
                currency.TypeCurrency == currency.TypeCurrency &&
                currency.Value == currency.Value;
        }
        public int CompareTo(Currency? currency)
        {
            if (currency == null) return -1;
            if (currency.TypeCurrency != TypeCurrency)
                return TypeCurrency.CompareTo(currency.TypeCurrency);
            return Convert.ToInt32(Value - currency.Value);
        }
    }
}
