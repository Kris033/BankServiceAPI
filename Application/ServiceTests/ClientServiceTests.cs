using Services;
using Models;
using Xunit;
using Models.Enums;
using Bogus;
using Bogus.DataSets;
using Models.Filters;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        
        [Fact]
        public void CreateClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            Assert.Throws<ArgumentException>(() => 
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                passportService.AddPassport(passport);
                var client = new Client(faker.Random.ReplaceNumbers("###-##A#-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                clientService.AddClient(client);
            });
        }
        [Fact]
        public void CreateClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            PersonService personService = new PersonService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            clientService.AddClient(client);

            //Assert
            Assert.NotNull(clientService.GetClient(client.Id));
        }
        [Fact]
        public void UpdateClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            clientService.AddClient(client);
            string newNumberPhone = faker.Random.ReplaceNumbers("###-####-###");
            client.ChangeNumberPhone(newNumberPhone);
            clientService.UpdateClient(client);

            //Assert
            Assert.True(clientService.GetClient(client.Id)?.NumberPhone == newNumberPhone);
        }
        [Fact]
        public void CreateAccountClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            clientService.AddClient(client);
            var amount = new Models.Currency(0, CurrencyType.LeiMD);
            currencyService.AddCurrency(amount);
            var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            clientService.AddAccount(account);

            //Assert
            Assert.NotNull(clientService.GetAccount(account.Id));
        }
        [Fact]
        public void DeleteClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            PersonService personService = new PersonService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            clientService.AddClient(client);
            clientService.DeleteClient(client.Id);

            //Assert
            Assert.Null(clientService.GetClient(client.Id));
        }
        [Fact]
        public void CreateAccountClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                passportService.AddPassport(passport);
                var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                clientService.AddClient(client);
                var amount = new Models.Currency(0, CurrencyType.LeiMD);
                currencyService.AddCurrency(amount);
                var account = new Account(client.Id, faker.Random.ReplaceNumbers("###! #### #### ####"), amount.Id);
                clientService.AddAccount(account);
            });
        }
        [Fact]
        public void UpdateAccountPositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            clientService.AddClient(client);
            var amount = new Models.Currency(0, CurrencyType.LeiMD);
            currencyService.AddCurrency(amount);
            var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            clientService.AddAccount(account);
            var newAccount = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            newAccount.Id = account.Id;
            clientService.ChangeAccountClient(newAccount);

            //Assert
            Assert.True(clientService.GetAccount(newAccount.Id)?.AccountNumber != account.AccountNumber);
        }
        [Fact]
        public void UpdateAccountThrowTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                passportService.AddPassport(passport);
                var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                clientService.AddClient(client);
                var amount = new Models.Currency(0, CurrencyType.LeiMD);
                currencyService.AddCurrency(amount);
                var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
                clientService.AddAccount(account);
                var newAccount = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ###o"), amount.Id);
                clientService.ChangeAccountClient(newAccount);
            });

        }
        [Fact]
        public void DeleteAccountClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var client = new Client(faker.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
            clientService.AddClient(client);
            var amount = new Models.Currency(0, CurrencyType.LeiMD);
            currencyService.AddCurrency(amount);
            var account = new Account(client.Id, faker.Random.ReplaceNumbers("#### #### #### ####"), amount.Id);
            clientService.AddAccount(account);
            clientService.DeleteAccountClient(account.Id);

            //Assert
            Assert.Null(clientService.GetAccount(account.Id));
        }
        [Fact]
        public void FilterGetClientsFromStorageTest()
        {
            //Arrange
            ClientService clientService = new ClientService();

            //Act
            var clients = clientService.GetClients(new GetFilterRequest() { CountItem = 1 });

            //Assert
            Assert.True(clients.Count == 1);
        }
    }
}
