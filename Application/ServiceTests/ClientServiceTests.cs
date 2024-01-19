using Services;
using Models;
using Xunit;
using Models.Enums;
using Bogus;
using Models.Requests;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        
        [Fact]
        public async Task CreateClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => 
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                await passportService.AddPassport(passport);
                var client = new Client(faker.Random.ReplaceNumbers("###-##A#-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                await clientService.AddClient(client);
            });
        }
        [Fact]
        public async Task CreateClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            PersonService personService = new PersonService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            await clientService.AddClient(client);

            //Assert
            Assert.NotNull(await clientService.GetClient(client.Id));
        }
        [Fact]
        public async Task UpdateClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            await clientService.AddClient(client);
            string newNumberPhone = faker.Random.ReplaceNumbers("###-####-###");
            client.ChangeNumberPhone(newNumberPhone);
            await clientService.UpdateClient(client);
            var changedClient = await clientService.GetClient(client.Id);

            //Assert
            Assert.True(changedClient?.NumberPhone == newNumberPhone);
        }
        [Fact]
        public async Task CreateAccountClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            await clientService.AddClient(client);
            var amount = new Models.Currency(faker.Random.Number(15000), faker.PickRandom<CurrencyType>());
            await currencyService.AddCurrency(amount);
            var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            await clientService.AddAccount(account);

            //Assert
            Assert.NotNull(await clientService.GetAccount(account.Id));
        }
        [Fact]
        public async Task DeleteClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            PersonService personService = new PersonService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            await clientService.AddClient(client);
            await clientService.DeleteClient(client.Id);

            //Assert
            Assert.Null(await clientService.GetClient(client.Id));
        }
        [Fact]
        public async Task CreateAccountClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                await passportService.AddPassport(passport);
                var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                await clientService.AddClient(client);
                var amount = new Models.Currency(0, CurrencyType.MDL);
                await currencyService.AddCurrency(amount);
                var account = new Account(client.Id, faker.Random.ReplaceNumbers("###! #### #### ####"), amount.Id);
                await clientService.AddAccount(account);
            });
        }
        [Fact]
        public async Task UpdateAccountPositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            await clientService.AddClient(client);
            var amount = new Models.Currency(0, CurrencyType.MDL);
            await currencyService.AddCurrency(amount);
            var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            await clientService.AddAccount(account);
            var newAccount = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            newAccount.Id = account.Id;
            await clientService.ChangeAccountClient(newAccount);
            var changedAccount = await clientService.GetAccount(newAccount.Id);

            //Assert
            Assert.True(changedAccount?.AccountNumber != account.AccountNumber);
        }
        [Fact]
        public async Task UpdateAccountThrowTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                await passportService.AddPassport(passport);
                var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                await clientService.AddClient(client);
                var amount = new Models.Currency(0, CurrencyType.MDL);
                await currencyService.AddCurrency(amount);
                var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
                await clientService.AddAccount(account);
                var newAccount = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ###o"), amount.Id);
                await clientService.ChangeAccountClient(newAccount);
            });
        }
        [Fact]
        public async Task DeleteAccountClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            await clientService.AddClient(client);
            var amount = new Models.Currency(0, CurrencyType.MDL);
            await currencyService.AddCurrency(amount);
            var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            await clientService.AddAccount(account);
            await clientService.DeleteAccountClient(account.Id);

            //Assert
            Assert.Null(await clientService.GetAccount(account.Id));
        }
        [Fact]
        public async Task FilterGetClientsFromStorageTest()
        {
            //Arrange
            ClientService clientService = new ClientService();

            //Act
            var clients = await clientService.GetClients(new GetFilterRequest() { CountItem = 1 });

            //Assert
            Assert.True(clients.Count == 1);
        }
    }
}
