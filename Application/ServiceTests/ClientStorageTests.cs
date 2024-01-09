using Models.Filters;
using Services;
using Services.Storage;
using Xunit;

namespace ServiceTests
{
    public class ClientStorageTests
    {
        [Fact]
        public void AddInStorageTest()
        {
            //Arrange
            ClientStorage clientStorage = new ClientStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var client = dataGenerator.GenerationClients(1).First();
            clientStorage.Add(client);

            //Assert
            Assert.Contains(client, clientStorage);
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
