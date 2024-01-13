using Bogus;
using Models;
using Models.Enums;
using Models.Filters;
using Services;
using Services.Storage;
using System.Security.Principal;
using Xunit;

namespace ServiceTests
{
    public class ClientStorageTests
    {
        [Fact]
        public void AddClientInStorageTest()
        {
            //Arrange
            IClientStorage clientStorage = new ClientStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var client = clientService.GetClients(request).First();
            if (client == null) 
                client = dataGenerator.GenerationClients(1).First();
            clientStorage.Add(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }
        [Fact]
        public void UpdateClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };
            Faker faker = new Faker();

            //Act
            var client = clientService.GetClients(request).First();
            if (client == null)
                client = dataGenerator.GenerationClients(1).First();
            clientStorage.Add(client);
            client.ChangeNumberPhone(faker.Random.ReplaceNumbers("###-####-###"));
            clientStorage.Update(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }
        [Fact]
        public void DeleteClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var client = clientService.GetClients(request).First();
            if (client == null)
                client = dataGenerator.GenerationClients(1).First();
            clientStorage.Add(client);
            clientStorage.Delete(client);

            //Assert
            Assert.DoesNotContain(client, clientStorage.Data.Keys);
        }
        [Fact]
        public void AddAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var client = clientService.GetClients(request).FirstOrDefault();
            if (client == null)
                client = dataGenerator.GenerationClients(1).First();
            var accountClient = clientService.GetAccounts(client.Id).FirstOrDefault();
            if (accountClient == null)
                accountClient = dataGenerator.GenerationAccounts(1, client).First();
            clientStorage.Add(client);
            clientStorage.AddAccount(accountClient);

            //Assert
            Assert.Contains(clientStorage.Data.Values, accountList => accountList.Any(a => a.Equals(accountClient)));
        }
        [Fact]
        public void UpdateAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };
            Faker faker = new Faker();

            //Act
            var client = clientService.GetClients(request).First();
            var accountClient = clientService.GetAccounts(client.Id).First();
            if (client == null)
                client = dataGenerator.GenerationClients(1).First();
            if (accountClient == null)
                accountClient = dataGenerator.GenerationAccounts(1, client).First();
            clientStorage.Add(client);
            clientStorage.AddAccount(accountClient);
            Account newAccount = new Account(
                accountClient.ClientId,
                "2432 2132 4555 2134",
                accountClient.CurrencyIdAmount)
            { Id = accountClient.Id };
            clientService.ChangeAccountClient(newAccount);
            clientStorage.UpdateAccount(newAccount);

            //Assert
            Assert.Contains(clientStorage.Data.Values, accountList => accountList.Any(a => a.Equals(newAccount)));
        }
        [Fact]
        public void DeleteAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService();
            GetFilterRequest request = new GetFilterRequest() { CountItem = 1 };

            //Act
            var client = clientService.GetClients(request).First();
            var accountClient = clientService.GetAccounts(client.Id).First();
            if (client == null)
                client = dataGenerator.GenerationClients(1).First();
            if (accountClient == null)
                accountClient = dataGenerator.GenerationAccounts(1).First();
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
