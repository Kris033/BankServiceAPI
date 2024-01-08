using Services;
using Models;
using Xunit;

namespace ServiceTests
{
    public class CreateClientTests
    {
        [Fact]
        public void CreateClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Assert
            Assert.Throws<ArgumentException>(() => 
            {
                //Act
                var client = new Client("123-4f65-a23", dataGenerator.GenerationPassport());
                clientService.AddClient(client);
            });
        }
        [Fact]
        public void CreateClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var client = new Client("123-4265-243", dataGenerator.GenerationPassport());
            clientService.AddClient(client);
        }
        [Fact]
        public void CreateAccountClientInServicePositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();

            //Act
            var client = clientService.GetFirstClient();
            clientService.AddAccount(client, new Account(client, "1234 2341 5553 2341", new Currency(0, CurrencyType.Euro)));
        }
        [Fact]
        public void CreateAccountClientInServiceThrowArgumentTest()
        {
            //Arrange
            ClientService clientService = new ClientService();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var client = clientService.GetFirstClient();
                clientService.AddAccount(client, new Account(client, "1234 2341 55a3 2341", new Currency(0, CurrencyType.Euro)));
            });
        }
    }
}
