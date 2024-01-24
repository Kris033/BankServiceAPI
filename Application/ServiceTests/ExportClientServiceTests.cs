using ExportTool;
using Models;
using Models.Exports;
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
            IExportService<Client, ClientExportModel> exportService = new ExportClientsService(path, fileCsv);
            ClientService clientService = new ClientService();

            //Act
            await exportService.ExportForCsv(await clientService.GetClients());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.NotEqual(0, fileStream.Length);
        }
        [Fact]
        public async Task ImportClientsFromCsvFileToDbTest()
        {
            //Arrange
            IExportService<Client, ClientExportModel> exportService = new ExportClientsService(@"..\..\..\..\ExportTool\ExcelInfo", "clients.csv");
            ClientService clientService = new ClientService();
            TestDataGenerator generator = new TestDataGenerator();

            //Act
            var clients = await clientService.GetClients();
            var newClient = await generator.GenerationClient();
            clients.Add(newClient);
            await exportService.ExportForCsv(clients);
            await exportService.ImportFromCsvToDb();

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
            IExportService<Client, ClientExportModel> exportService = new ExportClientsService(path, fileCsv);
            ClientService clientService = new ClientService();

            //Act
            await exportService.ExportForJson(await clientService.GetClients());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.True(fileStream.Length != 0);
        }
        [Fact]
        public async Task ImportClientsFromJsonFileToDbTest()
        {
            //Arrange
            IExportService<Client, ClientExportModel> exportService = new ExportClientsService(@"..\..\..\..\ExportTool\JsonFiles", "clients.json");
            ClientService clientService = new ClientService();
            TestDataGenerator generator = new TestDataGenerator();

            //Act
            var clients = await clientService.GetClients();
            var newClient = await generator.GenerationClient();
            clients.Add(newClient);
            string jsonResult = await exportService.ExportForJson(clients);
            await exportService.ImportFromJsonToDb(jsonResult);

            //Assert
            Assert.Contains(await clientService.GetClients(), c => c.PassportId == newClient.PassportId);
        }
    }
}
