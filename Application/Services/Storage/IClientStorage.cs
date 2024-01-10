using Models;

namespace Services.Storage
{
    public interface IClientStorage : IStorage<Client>
    {
        Dictionary<Client, List<Account>> Data { get; }
        void AddAccount(Account account);
        void UpdateAccount(Account account);
        void DeleteAccount(Account account);
    }
}
