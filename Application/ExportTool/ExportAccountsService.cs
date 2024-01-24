using CsvHelper;
using CsvHelper.Configuration;
using Models;
using Models.Exports;
using Newtonsoft.Json;
using Services;
using System.Globalization;
using System.Text;

namespace ExportTool
{
    public class ExportAccountsService : IExportService<Account, AccountExportModel>
    {
        public ExportAccountsService(string pathToDirectory, string fileName) 
        {
            PathToDirectory = pathToDirectory;
            FileName = fileName;
        }
        public ExportAccountsService() { }
        public string PathToDirectory { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public async Task ExportForCsv(List<Account> accounts)
        {
            var listAccount = await ConvertatorToExportListModel(accounts);
            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync(listAccount);
            await csvWriter.FlushAsync();
        }
        public async Task ImportFromCsvToDb()
        {
            string fullPath = Path.Combine(PathToDirectory, FileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var streamReader = new StreamReader(fullPath);
            using var csvReader = new CsvReader(streamReader, config);
            var accountService = new AccountService();
            var accountsList = new List<Account>();
            await csvReader.ReadAsync();
            csvReader.ReadHeader();
            while (await csvReader.ReadAsync())
            {
                var account = new Account(
                    csvReader.GetField<Guid>("ClientId"),
                    csvReader.GetField("Phone")!,
                    csvReader.GetField<Guid>("Id"))
                {
                    Id = csvReader.GetField<Guid>("Id")
                };
                accountsList.Add(account);
            }

            foreach (var account in accountsList)
                if (accountService.Get(account.Id) is null)
                    await accountService.Add(account);
        }
        public async Task<string> ExportForJson(List<Account> accounts)
        {
            var listAccount = await ConvertatorToExportListModel(accounts);
            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            var jsonAccounts = JsonConvert.SerializeObject(listAccount);
            await streamWriter.WriteLineAsync(jsonAccounts);
            return jsonAccounts;
        }

        public async Task ImportFromJsonToDb(string accountsJson)
        {
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var streamReader = new StreamReader(fullPath);
            var accountService = new AccountService();
            var accounts = JsonConvert.DeserializeObject<Account[]>(accountsJson);
            foreach (var account in accounts!)
                if (accountService.Get(account.Id) is null)
                    await accountService.Add(account);
        }
        public async Task<AccountExportModel> ConvertatorToExportModel(Account account)
            => new AccountExportModel(
                account.ClientId,
                account.AccountNumber,
                await new CurrencyService().Get(account.CurrencyId))
            { Id = account.Id };
        public async Task<List<AccountExportModel>> ConvertatorToExportListModel(List<Account> accounts)
        {
            List<AccountExportModel> accountExports = new List<AccountExportModel>();
            foreach (var account in accounts)
                accountExports
                    .Add(await ConvertatorToExportModel(account));
            return accountExports;
        }

    }
}
