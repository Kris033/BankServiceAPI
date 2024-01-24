using Models;

namespace Services.Interfaces
{
    public interface IAccountService : IModelService<Account>
    {
        Task<List<Account>> GetAccountsClient(Guid idClient);
        string GetBalance(Currency currency);
        Task Put(Guid idAccount, Currency currency);
        Task Remove(Guid idAccount, Currency currency);
    }
}
