using Bogus;
using Models;
using Models.Enums;
using Models.Requests;
using Services;
using Services.Storage;
using System.Security.Principal;
using Xunit;

namespace ServiceTests
{
    public class ClientStorageTests
    {
        [Fact]
        public async Task AddClientInStorageTest()
        {
            //Arrange
            IClientStorage clientStorage = new ClientStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault();
            if (client == null)
            {
                clients = await dataGenerator.GenerationClients(1);
                client = clients.First();
            }
            clientStorage.Add(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }
        [Fact]
        public async Task UpdateClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault();
            if (client == null)
            {
                clients = await dataGenerator.GenerationClients(1);
                client = clients.First();
            }
            clientStorage.Add(client);
            client.ChangeNumberPhone(faker.Random.ReplaceNumbers("###-####-###"));
            clientStorage.Update(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }
        [Fact]
        public async Task DeleteClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault();
            if (client == null)
            {
                clients = await dataGenerator.GenerationClients(1);
                client = clients.First();
            }
            clientStorage.Add(client);
            clientStorage.Delete(client);

            //Assert
            Assert.DoesNotContain(client, clientStorage.Data.Keys);
        }
        [Fact]
        public async Task AddAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault();
            if (client == null)
            {
                clients = await dataGenerator.GenerationClients(1);
                client = clients.First();
            }
            var accountsClient = await clientService.GetAccounts(client.Id);
            var accountClient = accountsClient.FirstOrDefault();
            if (accountClient == null) 
            {
                accountsClient = await dataGenerator.GenerationAccounts(1, client);
                accountClient = accountsClient.First();
            }
            clientStorage.Add(client);
            clientStorage.AddAccount(accountClient);

            //Assert
            Assert.Contains(clientStorage.Data.Values, accountList => accountList.Any(a => a.Equals(accountClient)));
        }
        [Fact]
        public async Task UpdateAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault();
            if (client == null)
            {
                clients = await dataGenerator.GenerationClients(1);
                client = clients.First();
            }
            var accountsClient = await clientService.GetAccounts(client.Id);
            var accountClient = accountsClient.FirstOrDefault();
            if (accountClient == null)
            {
                accountsClient = await dataGenerator.GenerationAccounts(1, client);
                accountClient = accountsClient.First();
            }
            clientStorage.Add(client);
            clientStorage.AddAccount(accountClient);
            Account newAccount = new Account(
                accountClient.ClientId,
                "2432 2132 4555 2134",
                accountClient.CurrencyIdAmount)
            { Id = accountClient.Id };
            await clientService.ChangeAccountClient(newAccount);
            clientStorage.UpdateAccount(newAccount);

            //Assert
            Assert.Contains(clientStorage.Data.Values, accountList => accountList.Any(a => a.Equals(newAccount)));
        }
        [Fact]
        public async Task DeleteAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault();
            if (client == null)
            {
                clients = await dataGenerator.GenerationClients(1);
                client = clients.First();
            }
            var accountsClient = await clientService.GetAccounts(client.Id);
            var accountClient = accountsClient.FirstOrDefault();
            if (accountClient == null)
            {
                accountsClient = await dataGenerator.GenerationAccounts(1, client);
                accountClient = accountsClient.First();
            }
            clientStorage.Add(client);
            clientStorage.AddAccount(accountClient);
            clientStorage.DeleteAccount(accountClient);

            //Assert
            Assert.DoesNotContain(clientStorage.Data.Values,
                accountList => accountList
                    .Any(account =>
                        account.Equals(accountClient)));
        }
    }
}
