using Models;
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

            //Act
            var client = dataGenerator.GenerationClients(1).First();
            clientStorage.Add(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }
        [Fact]
        public void UpdateClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage(dataGenerator.GenerationDictionaryAccount(10));

            //Act
            var client = clientStorage.Data.First().Key;
            client.ChangeNumberPhone("555-3413-888");
            clientStorage.Update(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }
        [Fact]
        public void DeleteClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage(dataGenerator.GenerationDictionaryAccount(10));

            //Act
            var client = dataGenerator.GenerationClients(1).First();
            clientStorage.Delete(client);

            //Assert
            Assert.DoesNotContain(client, clientStorage.Data.Keys);
        }
        [Fact]
        public void AddAccountClientInStorageTest()
        {
            //Arrange
            IClientStorage clientStorage = new ClientStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var client = dataGenerator.GenerationClients(1).First();
            clientStorage.Add(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }
        [Fact]
        public void UpdateAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage(dataGenerator.GenerationDictionaryAccount(10));

            //Act
            var clientAndAccounts = clientStorage.Data.First();
            var account = clientAndAccounts.Value.First();
            account.Put(new Currency(20, CurrencyType.Euro));
            clientStorage.UpdateAccount(account);

            //Assert
            Assert.Contains(account, clientStorage.Data[clientAndAccounts.Key]);
        }
        [Fact]
        public void DeleteAccountClientInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IClientStorage clientStorage = new ClientStorage(dataGenerator.GenerationDictionaryAccount(10));

            //Act
            var clientAndAccounts = clientStorage.Data.First();
            var account = clientAndAccounts.Value.First();
            clientStorage.DeleteAccount(account);

            //Assert
            Assert.DoesNotContain(account, clientStorage.Data[clientAndAccounts.Key]);
        }
        [Fact]
        public void FilterGetFromStorageTest()
        {
            //Arrange
            ClientService clientService = new ClientService();

            //Act
            var clients = clientService.GetClients(new GetFilterRequest() { DateBornTo = new DateOnly(1996, 1, 1) });

            //Assert
            Assert.DoesNotContain(clients, c => c.Passport!.DateBorn > new DateOnly(1996, 1, 1));
        }
    }
}
