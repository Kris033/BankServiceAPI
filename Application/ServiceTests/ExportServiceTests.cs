using Bogus;
using ExportTool;
using Services;
using Xunit;

namespace ServiceTests
{
    public class ExportServiceTests
    {
        [Fact]
        public void ExportClientsToCsvFileTest()
        {
            //Arrange
            ExportService exportService = new ExportService(@"..\..\..\..\ExportTool\ExcelInfo", "clients.csv");
            ClientService clientService = new ClientService();

            //Act
            exportService.ExportClientsForCsv(clientService.GetClients());
        }
        [Fact]
        public void ImportClientsFromCsvFileToDbTest()
        {
            //Arrange
            ExportService exportService = new ExportService(@"..\..\..\..\ExportTool\ExcelInfo", "clients.csv");
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            TestDataGenerator generator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var clients = clientService.GetClients();
            var passport = generator.GenerationPassport();
            passportService.AddPassport(passport);
            Models.Client newClient = new Models.Client(
                    faker.Random.ReplaceNumbers("###-####-###"),
                    passport.Id,
                    passport.GetFullName(),
                    passport.GetAge());
            clients.Add(newClient);
            exportService.ExportClientsForCsv(clients);
            exportService.ImportClientsFromCsv();

            //Assert
            Assert.Contains(clientService.GetClients(), c => c.PassportId == newClient.PassportId);
        }
    }
}
