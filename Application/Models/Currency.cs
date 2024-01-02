namespace Models
{
    public struct Currency
    {
        public static decimal LeiToDollarExchangeRate { get; private set; } = 0.0058M;
        public static decimal DollarToLeiExchangeRate { get; private set; } = 17.30M;
        public static decimal LeiToEuroExchangeRate { get; private set; } = 0.0052M;
        public static decimal EuroToLeiExchangeRate { get; private set; } = 19.16M;
        public static decimal EuroToDollarExchangeRate { get; private set; } = 1.11M;
        public static decimal DollarToEuroExchangeRate { get; private set; } = 0.90M;

        public Currency(int value, CurrencyType currencyType) 
        {
            Value = value;
            TypeCurrency = currencyType;
        }
        public decimal Value { get; private set; }
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
    }
    public enum CurrencyType
    {
        Euro,
        Dollar,
        LeiMD
    }
}
