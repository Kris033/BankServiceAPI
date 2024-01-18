using Bogus;
using ExportTool;
using Models;
using Models.Requests;
using Services;
using Models.Enums;
using Xunit;

namespace ServiceTests
{
    public class ThreadAndTaskTests
    {
        [Fact]
        public void ImportWithThreadClientInDbTest()
        {
            //Arrange
            ClientService clientService = new ClientService();
            Mutex mutex = new Mutex(); 
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
                mutex.WaitOne();
                ExportClientsService exportService = new ExportClientsService(@"..\..\..\..\ExportTool\ExcelInfo", "clients.csv");
                
                await exportService.ImportClientsFromCsv();
                mutex.ReleaseMutex();
            }
            async void ExportClients()
            {
                Thread.Sleep(90);
                mutex.WaitOne();
                ExportClientsService exportService = new ExportClientsService(@"..\..\..\..\ExportTool\ExcelInfo", "clientsFromDb.csv");
                
                await exportService.ExportClientsForCsv(await clientService.GetClients());
                mutex.ReleaseMutex();
            }
        }
        [Fact]
        public void ParallelPutCurrencyInAccount()
        {
            //Arrange
            Currency currency = new Currency(0, Models.Enums.CurrencyType.Dollar);
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
            

            void PutInAccount()
            {
                mutex.WaitOne();
                Currency currencyPut = new Currency(100, Models.Enums.CurrencyType.Dollar);
                for (int i = 0; i < 10; i++)
                {
                    if (currency.TypeCurrency != currencyPut.TypeCurrency)
                        currencyPut.ExChange(currency.TypeCurrency);
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
            ClientService clientService = new ClientService();
            CurrencyService currencyService = new CurrencyService();

            //Act
            var clients = await clientService.GetClients();
            var client = clients.First();
            var accounts = await clientService.GetAccounts(client.Id);
            var account = accounts.First();
            var currencyAccount = await currencyService.GetCurrency(account.CurrencyIdAmount);
            await rateUpdater.InterestRateCalculation();
            var newInformationCurrencyAccount = await currencyService.GetCurrency(account.CurrencyIdAmount);

            //Assert
            Assert.NotEqual(newInformationCurrencyAccount!.Value, currencyAccount!.Value);
        }
        [Fact]
        public async void CashDispanserTest()
        {
            //Arrange
            Dictionary<Account, OperationAccountRequest> accountWithRequestOperation = new Dictionary<Account, OperationAccountRequest>();
            List<Currency> accountsCurrencyBefore;
            List<Currency> accountsCurrencyAfter;
            ClientService clientService = new ClientService();
            CurrencyService currencyService = new CurrencyService();
            CashDispanserService cashDispanserService;
            Faker faker = new Faker();

            //Act
            var clients = await clientService.GetClients();
            var client = clients.First();
            var accounts = await clientService.GetAccounts(client.Id);
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
            var listResponse = cashDispanserService.CashingOutClients();
            accountsCurrencyAfter = await GetListCurrency(accounts);
            foreach (var item in accountWithRequestOperation)
            {
                if (item.Value.OperationAccount == TypeOperationAccount.Withdraw 
                    && accountsCurrencyBefore.First(c => c.Id == item.Key.CurrencyIdAmount).Value 
                    < item.Value.Currency!.Value)
                    continue;
                switch (item.Value.OperationAccount)
                {
                    case TypeOperationAccount.Withdraw:
                    case TypeOperationAccount.Put:
                        //Assert
                        Assert.NotEqual(
                            accountsCurrencyBefore.First(c => c.Id == item.Key.CurrencyIdAmount).Value,
                            accountsCurrencyAfter.First(c => c.Id == item.Key.CurrencyIdAmount).Value);
                        break;
                }
            }
            async Task<List<Currency>> GetListCurrency(List<Account> accounts)
            {
                var currencysAmount = new List<Currency>();
                foreach (var account in accounts)
                {
                    currencysAmount.Add(await currencyService.GetCurrency(account.CurrencyIdAmount));
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