using Models;
using Models.Requests;

namespace Services.Interfaces
{
    public interface IClientService : IModelService<Client>
    {
        Task<List<Client>> GetClients(GetFilterRequest? filterRequest = null);
        Task<Client> GetYoungestClient();
        Task<Client> GetOldestClient();
        Task<int> GetAverrageAgeClients();
        string GetInformation(Client client);
    }
}
