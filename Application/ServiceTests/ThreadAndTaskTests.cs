using Bogus;
using ExportTool;
using Models;
using Models.Requests;
using Services;
using Models.Enums;
using Xunit;
using Services.Interfaces;

namespace ServiceTests
{
    public class ThreadAndTaskTests
    {
        [Fact]
        public void ImportWithThreadClientInDbTest()
        {
            //Arrange
            IClientService clientService = new ClientService();
            Mutex mutex1 = new Mutex(); 
            Mutex mutex2 = new Mutex();
            Thread thread1 = new Thread(ImportClients);
            Thread thread2 = new Thread(ExportClients);

            //Act
            thread1.Start();
            thread2.Start();

            while (true)
            {
                if(!thread1.IsAlive && !thread2.IsAlive)
                {
                    //Assert
                    Assert.True(!thread1.IsAlive && !thread2.IsAlive);
                    break;
                }
                Thread.Sleep(500);
            }
            async void ImportClients()
            {
                Thread.Sleep(100);
                mutex1.WaitOne();
                ExportClientsService exportService = new ExportClientsService(@"..\..\..\..\ExportTool\ExcelInfo", "clients.csv");
                
                await exportService.ImportFromCsvToDb();
                mutex1.ReleaseMutex();
            }
            async void ExportClients()
            {
                Thread.Sleep(90);
                mutex2.WaitOne();
                ExportClientsService exportService = new ExportClientsService(@"..\..\..\..\ExportTool\ExcelInfo", "clientsFromDb.csv");
                
                await exportService.ExportForCsv(await clientService.GetClients());
                mutex2.ReleaseMutex();
            }
        }
        [Fact]
        public void ParallelPutCurrencyInAccount()
        {
            //Arrange
            ICurrencyService currencyService = new CurrencyService();
            Currency currency = new Currency(0, Models.Enums.CurrencyType.USD);
            Faker faker = new Faker();
            Mutex mutex = new Mutex();
            Thread thread1 = new Thread(PutInAccount);
            Thread thread2 = new Thread(PutInAccount);

            //Act
            thread1.Start();
            thread2.Start();
            while(true)
            {
                
                if(!thread1.IsAlive && !thread2.IsAlive)
                {
                    //Assert
                    Assert.Equal(2000, currency.Value);
                    break;
                }
                Thread.Sleep(1000);
            }
            

            async void PutInAccount()
            {
                mutex.WaitOne();

                Currency currencyPut = new Currency(100, Models.Enums.CurrencyType.USD);
                for (int i = 0; i < 10; i++)
                {
                    if (currency.TypeCurrency != currencyPut.TypeCurrency)
                        await currencyService.ExChange(currencyPut, currency.TypeCurrency);
                    Thread.Sleep(100);
                    currency.ChangeValue(currency.Value + currencyPut.Value);
                }
                mutex.ReleaseMutex();
            }
        }
        [Fact]
        public async void InterestRateCalculationTest()
        {
            //Arrange
            RateUpdater rateUpdater = new RateUpdater();
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            CurrencyService currencyService = new CurrencyService();

            //Act
            var clients = await clientService.GetClients();
            var client = clients.First();
            var accounts = await accountService.GetAccountsClient(client.Id);
            var account = accounts.First();
            var currencyAccount = await currencyService.Get(account.CurrencyId);
            await rateUpdater.InterestRateCalculation();
            var newInformationCurrencyAccount = await currencyService.Get(account.CurrencyId);

            //Assert
            Assert.NotEqual(newInformationCurrencyAccount!.Value, currencyAccount!.Value);
        }
        [Fact]
        public async Task CashDispanserTest()
        {
            //Arrange
            Dictionary<Account, OperationAccountRequest> accountWithRequestOperation = new Dictionary<Account, OperationAccountRequest>();
            List<Currency> accountsCurrencyBefore;
            List<Currency> accountsCurrencyAfter;
            IClientService clientService = new ClientService();
            IAccountService accountService = new AccountService();
            ICurrencyService currencyService = new CurrencyService();
            CashDispanserService cashDispanserService;
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients();
            var client = clients.First();
            var accounts = await accountService.GetAccountsClient(client.Id);
            accountsCurrencyBefore = await GetListCurrency(accounts);
            for (int i = 0; i < accounts.Count; i++)
            {
                var typeOperation = faker.PickRandom<TypeOperationAccount>();
                Currency? currency = typeOperation switch 
                {
                    TypeOperationAccount.Put => new Currency(faker.Random.Number(1000), accountsCurrencyBefore[i].TypeCurrency),
                    TypeOperationAccount.Withdraw => new Currency(faker.Random.Number(0, (int)accountsCurrencyBefore[i].Value), accountsCurrencyBefore[i].TypeCurrency),
                    _ => null
                };
                accountWithRequestOperation.Add(accounts[i], new OperationAccountRequest(accounts[i].Id, typeOperation, currency));
            }
            cashDispanserService =
                new CashDispanserService(accountWithRequestOperation);
            var listResponse = await cashDispanserService.CashingOutClients();
            accountsCurrencyAfter = await GetListCurrency(await accountService.GetAccountsClient(client.Id));
            foreach (var item in accountWithRequestOperation)
            {
                if (item.Value.OperationAccount == TypeOperationAccount.Withdraw)
                {
                    var account = accountsCurrencyBefore.First(c => c.Id == item.Key.CurrencyId);
                    if (account.TypeCurrency != item.Value.Currency?.TypeCurrency
                        || account.Value < item.Value.Currency?.Value)
                        continue;
                }
                switch (item.Value.OperationAccount)
                {
                    case TypeOperationAccount.Withdraw:
                    case TypeOperationAccount.Put:
                        //Assert
                        Assert.NotEqual(
                            accountsCurrencyBefore.First(c => c.Id == item.Key.CurrencyId).Value,
                            accountsCurrencyAfter.First(c => c.Id == item.Key.CurrencyId).Value);
                        break;
                }
            }
            async Task<List<Currency>> GetListCurrency(List<Account> accounts)
            {
                var currencysAmount = new List<Currency>();
                foreach (var account in accounts)
                {
                    currencysAmount.Add(await currencyService.Get(account.CurrencyId));
                }
                return currencysAmount;
            }
        }
        [Fact]
        public void WorkThreadInBackgroundTest()
        {
            //Arrange
            RateUpdater rateUpdater = new RateUpdater();

            //Act
            rateUpdater.CreateThread();

            //Assert
            Assert.True(rateUpdater.Thread!.IsAlive);
        }
        [Fact]
        public void StopWorkThreadInBackgroundTest()
        {
            //Arrange
            RateUpdater rateUpdater = new RateUpdater();

            //Act
            rateUpdater.ThreadAbort();

            //Assert
            Assert.Null(rateUpdater.Thread);
        }
    }
}