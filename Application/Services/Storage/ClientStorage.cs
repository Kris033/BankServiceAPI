using Models;
using System.Collections;

namespace Services.Storage
{
    public class ClientStorage : IEnumerable<Client>
    {
        private Client[] _clients;
        public ClientStorage(Client[] clients) => _clients = clients;
        public ClientStorage() => _clients = new Client[0];
        public Client this[int index] => _clients[index];
        public void Add(Client client)
        {
            Array.Resize(ref _clients, _clients.Length + 1);
            _clients[^1] = client;
        }
        public IEnumerator<Client> GetEnumerator()
            => ((IEnumerable<Client>)_clients).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
