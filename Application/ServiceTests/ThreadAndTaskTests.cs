using Bogus;
using ExportTool;
using Models;
using Services;
using Xunit;

namespace ServiceTests
{
    public class ThreadAndTaskTests
    {
        [Fact]
        public void ImportWithThreadClientInDbTest()
        {
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
            void ImportClients()
            {
                Thread.Sleep(100);
                mutex.WaitOne();
                ExportService exportService = new ExportService(@"..\..\..\..\ExportTool\ExcelInfo", "clients.csv");
                
                exportService.ImportClientsFromCsv();
                mutex.ReleaseMutex();
            }
            void ExportClients()
            {
                Thread.Sleep(90);
                mutex.WaitOne();
                ExportService exportService = new ExportService(@"..\..\..\..\ExportTool\ExcelInfo", "clientsFromDb.csv");
                
                exportService.ExportClientsForCsv(clientService.GetClients());
                mutex.ReleaseMutex();
            }
        }
        [Fact]
        public void ParallelPutCurrencyInAccount()
        {
            //Arr
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
    }
}
