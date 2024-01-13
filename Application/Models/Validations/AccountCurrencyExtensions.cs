using Models.Enums;

namespace Models.Validations
{
    public static partial class AccountCurrencyExtensions
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
