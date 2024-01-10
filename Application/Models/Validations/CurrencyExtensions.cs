namespace Models.Validations
{
    public static partial class CurrencyExtensions
    {
        public static void Validation(this Currency currency)
        {
            if (currency.Value < 0)
                throw new ArgumentOutOfRangeException("Значение валюты не может быть в отрицательном значении");
        }
    }
}
