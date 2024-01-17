using BankDbConnection;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class RateUpdater
    {

        public RateUpdater() { }
        public Thread? Thread { get; private set; }
        public async Task InterestRateCalculation()
        {
            ClientService clientService = new ClientService();
            AccountService accountService = new AccountService();
            var clients = clientService.GetClients().Result;
            using var db = new BankContext();
            foreach (var client in clients)
            {
                var accountsClient = clientService.GetAccounts(client.Id).Result;
                foreach (var account in accountsClient)
                {
                    var currencyAccount = await db.Currency.FirstOrDefaultAsync(c => c.Id == account.CurrencyIdAmount);
                    currencyAccount?.ChangeValue(currencyAccount.Value + (currencyAccount.Value * 0.03m));
                    if (currencyAccount != null)
                        account.OnPropertyChanged($"Вам была начисленна процентная ставка 3%. {accountService.GetBalance(currencyAccount)}");
                    await db.SaveChangesAsync();
                }
            }
        }
        public void CreateThread()
        {
            if (Thread == null)
            {
                Thread = new Thread(async () =>
                {
                    while (true)
                    {
                        if (DateTime.Today.Day == 1)
                             await InterestRateCalculation();
                        Thread.Sleep(new TimeSpan(24, 0, 0));
                    }
                });
                Thread.IsBackground = false;
                Thread.Start();
            }
        }
        public void ThreadAbort()
        {
            Thread?.Abort();
            Thread = null;
        }
    }
}
