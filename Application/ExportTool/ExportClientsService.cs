using Models;
using Models.Exports;
using CsvHelper;
using System.Globalization;
using System.Text;
using CsvHelper.Configuration;
using Services;
using Newtonsoft.Json;

namespace ExportTool
{
    public class ExportClientsService : IExportService<Client, ClientExportModel>
    {
        public string PathToDirectory { get; set; }
        public string FileName { get; set; }
        public ExportClientsService(string pathToDirectory, string csvFileName) 
        {
            PathToDirectory = pathToDirectory;
            FileName = csvFileName;
        }
        public ExportClientsService() { }
        public async Task ExportForCsv(List<Client> clients)
        {
            var listClient = await ConvertatorToExportListModel(clients);
            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync(listClient);
            await csvWriter.FlushAsync();
        }
        public async Task ImportFromCsvToDb()
        {
            string fullPath = Path.Combine(PathToDirectory, FileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var streamReader = new StreamReader(fullPath);
            using var csvReader = new CsvReader(streamReader, config);
            var clientService = new ClientService();
            var clientsList = new List<Client>();
            var clientsListFromDb = await clientService.GetClients();
            await csvReader.ReadAsync();
            csvReader.ReadHeader();
            while (await csvReader.ReadAsync())
            {
                var clientCsv = new Client(
                    csvReader.GetField("Phone")!,
                    csvReader.GetField<Guid>("PassportId"),
                    csvReader.GetField<string>("Name")!,
                    csvReader.GetField<int>("Age"))
                {
                    Id = csvReader.GetField<Guid>("Id"),
                    InBlackList = csvReader.GetField<bool>("InBlackList")
                };
                clientsList.Add(clientCsv);
            }
            foreach (var clientCsv in clientsList)
                if (!clientsListFromDb.Any(c => c.PassportId == clientCsv.PassportId))
                    await clientService.Add(clientCsv);
        }
        public async Task<string> ExportForJson(List<Client> clients)
        {
            var listClient = await ConvertatorToExportListModel(clients);
            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            var jsonClients = JsonConvert.SerializeObject(listClient);
            await streamWriter.WriteLineAsync(jsonClients);
            return jsonClients;
        }
        public async Task ImportFromJsonToDb(string clientsJson)
        {
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var streamReader = new StreamReader(fullPath);
            var clientService = new ClientService();
            var clientsListFromDb = await clientService.GetClients();
            var clients = JsonConvert.DeserializeObject<Client[]>(clientsJson);

            foreach (var client in clients!)
                if (!clientsListFromDb.Any(c => c.PassportId == client.PassportId))
                    await clientService.Add(client);
        }
        public async Task<ClientExportModel> ConvertatorToExportModel(Client client)
            => new ClientExportModel(
                client.PassportId,
                client.Name,
                client.Age,
                client.NumberPhone,
                client.InBlackList,
                await new ExportAccountsService()
                .ConvertatorToExportListModel(
                    await new AccountService()
                    .GetAccountsClient(client.Id)))
            { Id = client.Id };
        public async Task<List<ClientExportModel>> ConvertatorToExportListModel(List<Client> clients)
        {
            List<ClientExportModel> clientExports = new List<ClientExportModel>();
            foreach (var client in clients)
                clientExports
                    .Add(await ConvertatorToExportModel(client));
            return clientExports;
        }
        
    }
}
