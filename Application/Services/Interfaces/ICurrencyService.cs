using Models;
using Models.Enums;

namespace Services.Interfaces
{
    public interface ICurrencyService : IModelService<Currency>
    {
        Task ExChange(Currency currency, CurrencyType currencyTypeExChange);
    }
}
