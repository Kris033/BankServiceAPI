using Bogus;
using Models;
using Models.Requests;
using Services;
using Services.Interfaces;
using Services.Storage;
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
            IClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault()
                ?? await dataGenerator.GenerationClient();
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
            IClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault()
                ?? await dataGenerator.GenerationClient();
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
            IClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault()
                ?? await dataGenerator.GenerationClient();
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
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault()
                ?? await dataGenerator.GenerationClient();
            var accountsClient = await accountService.GetAccountsClient(client.Id);
            var accountClient = accountsClient.FirstOrDefault()
                ?? await dataGenerator.GenerationAccount(client);
            clientStorage.Add(client);
            await clientStorage.AddAccount(accountClient);

            //Assert
            Assert.Contains(clientStorage.Data.Values, accountList => accountList.Any(a => a.Equals(accountClient)));
        }
        [Fact]
        public async Task UpdateAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            IAccountService accountService = new AccountService();
            IClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault()
                ?? await dataGenerator.GenerationClient();
            var accountsClient = await accountService.GetAccountsClient(client.Id);
            var accountClient = accountsClient.FirstOrDefault()
                ?? await dataGenerator.GenerationAccount(client);
            clientStorage.Add(client);
            await clientStorage.AddAccount(accountClient);
            Account newAccount = new Account(
                accountClient.ClientId,
                "2432 2132 4555 2134",
                accountClient.CurrencyId)
            { Id = accountClient.Id };
            await accountService.Update(newAccount);
            await clientStorage.UpdateAccount(newAccount);

            //Assert
            Assert.Contains(clientStorage.Data.Values, accountList => accountList.Any(a => a.Equals(newAccount)));
        }
        [Fact]
        public async Task DeleteAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            IAccountService accountService = new AccountService();
            IClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var clients = await clientService.GetClients(request);
            var client = clients.FirstOrDefault()
                ?? await dataGenerator.GenerationClient();
            var accountsClient = await accountService.GetAccountsClient(client.Id);
            var accountClient = accountsClient.FirstOrDefault()
                ?? await dataGenerator.GenerationAccount(client);
            clientStorage.Add(client);
            await clientStorage.AddAccount(accountClient);
            await clientStorage.DeleteAccount(accountClient);

            //Assert
            Assert.DoesNotContain(clientStorage.Data.Values,
                accountList => accountList
                    .Any(account =>
                        account.Equals(accountClient)));
        }
    }
}
