using Models;
using CsvHelper;
using System.Globalization;
using System.Text;
using CsvHelper.Configuration;
using Services;

namespace ExportTool
{
    public class ExportService
    {
        private string _pathToDirectory { get; set; }
        private string _csvFileName { get; set; }
        public ExportService(string pathToDirectory, string csvFileName) 
        {
            _pathToDirectory = pathToDirectory;
            _csvFileName = csvFileName;
        }
        public async Task ExportClientsForCsv(List<Client> clients)
        {
            var listClient = ConvertatorClients(clients);
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
            using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync(listClient);
            await csvWriter.FlushAsync();
        }
        public async Task ImportClientsFromCsv()
        {
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
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
                    InBlackList = csvReader.GetField<bool>("InBlackList")
                };
                clientsList.Add(clientCsv);
            }
            foreach (var clientCsv in clientsList)
            {
                if (!clientsListFromDb.Any(c => c.PassportId == clientCsv.PassportId))
                {
                    await clientService.AddClient(clientCsv);
                }
            }
        }
        private List<ClientModelCsv> ConvertatorClients(List<Client> clients) 
            => clients
                .Select(c => 
                new ClientModelCsv(
                    c.PassportId,
                    c.Name,
                    c.Age,
                    c.NumberPhone,
                    c.InBlackList)).ToList();
        
    }
}
