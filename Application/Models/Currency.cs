using Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Models
{
    [Table("currency")]
    public class Currency : IComparable<Currency>
    {
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
        public CurrencyType TypeCurrency { get; set; }

        public void ChangeValue(decimal valueNow)
            => Value = Math.Round(valueNow, 2);
        public override int GetHashCode()
        {
            return Id.GetHashCode()
                + TypeCurrency.GetHashCode()
                + Value.GetHashCode();
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null || obj is not Currency)
                return false;
            var currency = (Currency)obj;
            return 
                currency.Id == Id &&
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
