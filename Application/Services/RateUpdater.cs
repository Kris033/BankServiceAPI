using BankDbConnection;

namespace Services
{
    public class RateUpdater
    {

        public RateUpdater() { }
        public Thread? Thread { get; private set; }
        public void InterestRateCalculation()
        {
            ClientService clientService = new ClientService();
            AccountService accountService = new AccountService();
            var clients = clientService.GetClients();
            using var db = new BankContext();
            foreach (var client in clients)
            {
                var accountsClient = clientService.GetAccounts(client.Id);
                foreach (var account in accountsClient)
                {
                    var currencyAccount = db.Currency.FirstOrDefault(c => c.Id == account.CurrencyIdAmount);
                    currencyAccount?.ChangeValue(currencyAccount.Value + (currencyAccount.Value * 0.03m));
                    if (currencyAccount != null)
                        account.OnPropertyChanged($"Вам была начисленна процентная ставка 3%. {accountService.Balance(currencyAccount)}");
                    db.SaveChanges();
                }
            }
        }
        public void CreateThread()
        {
            if (Thread == null)
            {
                Thread = new Thread(() =>
                {
                    while (true)
                    {
                        if (DateTime.Today.Day == 1)
                            InterestRateCalculation();
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
