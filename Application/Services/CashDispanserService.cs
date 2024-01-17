using BankDbConnection;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Models.Requests;
using Models.Response;

namespace Services
{
    public class CashDispanserService
    {
        public Dictionary<Account, OperationAccountRequest> DictionaryAccountWithOperation { get; private set; }
        public CashDispanserService(
            Dictionary<Account, OperationAccountRequest> dictionaryAccountWithOperation) 
        {
            DictionaryAccountWithOperation = dictionaryAccountWithOperation;
        }
        public async Task<List<GetAccountResponse>> CashingOutClients()
        {
            AccountService accountService = new AccountService();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            List<GetAccountResponse> accountsResponses = new List<GetAccountResponse>();
            foreach (var item in DictionaryAccountWithOperation)
            {
                Task task = item.Value.OperationAccount switch
                {
                    TypeOperationAccount.Put => new Task(async () =>
                    {
                        if (item.Value.Currency == null)
                            cancellationTokenSource.Cancel();
                        if (cancellationToken.IsCancellationRequested)
                            return;
                        await accountService.Put(item.Key.Id, item.Value.Currency!);
                    }, cancellationToken),
                    TypeOperationAccount.Withdraw => new Task(async () =>
                    {
                        if (item.Value.Currency == null)
                            cancellationTokenSource.Cancel();
                        if (cancellationToken.IsCancellationRequested)
                            return;
                        await accountService.Remove(item.Key.Id, item.Value.Currency!);
                    }, cancellationToken),
                    TypeOperationAccount.CheckBalance => new Task(async () =>
                    {
                        using var db = new BankContext();
                        var currencyAccount = await db.Currency.FirstOrDefaultAsync(c => c.Id == item.Key.CurrencyIdAmount);
                        if (currencyAccount == null)
                            cancellationTokenSource.Cancel();
                        if (cancellationToken.IsCancellationRequested)
                            return;
                        accountsResponses.Add(new GetAccountResponse(item.Key.Id, currencyAccount!));
                    }, cancellationToken),
                    _ => new Task(() => { })
                };
                task.Start();
                await task.WaitAsync(cancellationToken);
            }
            return accountsResponses;
        }
    }
}
