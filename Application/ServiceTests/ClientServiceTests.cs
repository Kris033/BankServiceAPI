using Services;
using Models;
using Xunit;
using Bogus;
using Models.Requests;
using Services.Interfaces;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task CreateClientInServiceThrowArgumentTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => 
            {
                //Act
                var passport = await dataGenerator.GenerationPassport();
                var client = new Client(faker.Random.ReplaceNumbers("###-##A#-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                await clientService.Add(client);
            });
        }
        [Fact]
        public async Task CreateClientInServicePositiveTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var client = await dataGenerator.GenerationClient();

            //Assert
            Assert.NotNull(await clientService.Get(client.Id));
        }
        [Fact]
        public async Task UpdateClientInServicePositiveTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var client = await dataGenerator.GenerationClient();
            string newNumberPhone = faker.Random.ReplaceNumbers("###-####-###");
            client.ChangeNumberPhone(newNumberPhone);
            await clientService.Update(client);
            var changedClient = await clientService.Get(client.Id);

            //Assert
            Assert.True(changedClient?.NumberPhone == newNumberPhone);
        }
        [Fact]
        public async Task CreateAccountClientInServicePositiveTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var client = await dataGenerator.GenerationClient();
            var account = await dataGenerator.GenerationAccount(client);

            //Assert
            Assert.NotNull(await accountService.Get(account.Id));
        }
        [Fact]
        public async Task DeleteClientInServicePositiveTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var client = await dataGenerator.GenerationClient();
            await clientService.Delete(client.Id);

            //Assert
            Assert.Null(await clientService.Get(client.Id));
        }
        [Fact]
        public async Task CreateAccountClientInServiceThrowArgumentTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                var client = await dataGenerator.GenerationClient();
                var amount = await dataGenerator.GenerationCurrency();
                var account = new Account(client.Id, faker.Random.ReplaceNumbers("###! #### #### ####"), amount.Id);
                await accountService.Add(account);
            });
        }
        [Fact]
        public async Task UpdateAccountPositiveTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var client = await dataGenerator.GenerationClient();
            var account = await dataGenerator.GenerationAccount(client);
            var newAccount = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), account.CurrencyId);
            newAccount.Id = account.Id;
            await accountService.Update(newAccount);
            var changedAccount = await accountService.Get(newAccount.Id);

            //Assert
            Assert.True(changedAccount?.AccountNumber != account.AccountNumber);
        }
        [Fact]
        public async Task UpdateAccountThrowTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                var client = await dataGenerator.GenerationClient();
                var account = await dataGenerator.GenerationAccount(client);
                var newAccount = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ###o"), account.CurrencyId);
                await accountService.Update(newAccount);
            });
        }
        [Fact]
        public async Task DeleteAccountClientInServiceThrowArgumentTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var client = await dataGenerator.GenerationClient();
            var account = await dataGenerator.GenerationAccount(client);
            await accountService.Delete(account.Id);

            //Assert
            Assert.Null(await accountService.Get(account.Id));
        }
        [Fact]
        public async Task FilterGetClientsFromStorageTest()
        {
            //Arrange
            IClientService clientService = new ClientService();

            //Act
            var clients = await clientService.GetClients(new GetFilterRequest() { CountItem = 1 });

            //Assert
            Assert.True(clients.Count == 1);
        }
    }
}
