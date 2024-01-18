using Bogus;
using ExportTool;
using Services;
using Xunit;

namespace ServiceTests
{
    public class ExportClientServiceTests
    {
        [Fact]
        public async Task ExportClientsToCsvFileTest()
        {
            //Arrange
            string path = @"..\..\..\..\ExportTool\ExcelInfo";
            string fileCsv = "clients.csv";
            string fullPath = Path.Combine(path, fileCsv);
            ExportClientsService exportService = new ExportClientsService(path, fileCsv);
            ClientService clientService = new ClientService();

            //Act
            await exportService.ExportClientsForCsv(await clientService.GetClients());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.True(fileStream.Length != 0);
        }
        [Fact]
        public async Task ImportClientsFromCsvFileToDbTest()
        {
            //Arrange
            ExportClientsService exportService = new ExportClientsService(@"..\..\..\..\ExportTool\ExcelInfo", "clients.csv");
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            TestDataGenerator generator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients();
            var passport = generator.GenerationPassport();
            await passportService.AddPassport(passport);
            Models.Client newClient = new Models.Client(
                    faker.Random.ReplaceNumbers("###-####-###"),
                    passport.Id,
                    passport.GetFullName(),
                    passport.GetAge());
            clients.Add(newClient);
            await exportService.ExportClientsForCsv(clients);
            await exportService.ImportClientsFromCsv();

            //Assert
            Assert.Contains(await clientService.GetClients(), c => c.PassportId == newClient.PassportId);
        }
        [Fact]
        public async Task ExportClientsToJsonFileTest()
        {
            //Arrange
            string path = @"..\..\..\..\ExportTool\JsonFiles";
            string fileCsv = "clients.json";
            string fullPath = Path.Combine(path, fileCsv);
            ExportClientsService exportService = new ExportClientsService(path, fileCsv);
            ClientService clientService = new ClientService();

            //Act
            await exportService.ExportClientsToJson(await clientService.GetClients());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.True(fileStream.Length != 0);
        }
        [Fact]
        public async Task ImportClientsFromJsonFileToDbTest()
        {
            //Arrange
            ExportClientsService exportService = new ExportClientsService(@"..\..\..\..\ExportTool\JsonFiles", "clients.json");
            ClientService clientService = new ClientService();
            PassportService passportService = new PassportService();
            TestDataGenerator generator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients();
            var passport = generator.GenerationPassport();
            await passportService.AddPassport(passport);
            Models.Client newClient = new Models.Client(
                    faker.Random.ReplaceNumbers("###-####-###"),
                    passport.Id,
                    passport.GetFullName(),
                    passport.GetAge());
            clients.Add(newClient);
            string jsonResult = await exportService.ExportClientsToJson(clients);
            await exportService.ImportClientsFromJson(jsonResult);

            //Assert
            Assert.Contains(await clientService.GetClients(), c => c.PassportId == newClient.PassportId);
        }
    }
}
