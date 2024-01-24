namespace Models.Exports
{
    public class AccountExportModel
    {
        public AccountExportModel(Guid clientId, string accountNumber, Currency amount) 
        {
            ClientId = clientId;
            AccountNumber = accountNumber;
            Amount = amount;
        }
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string AccountNumber { get; set; }
        public Currency Amount { get; set; }
    }
}