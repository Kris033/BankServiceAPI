using Models;

namespace Services.Storage
{
    public interface IClientStorage : IStorage<Client>
    {
        Dictionary<Client, List<Account>> Data { get; }
        Task AddAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(Account account);
    }
}
