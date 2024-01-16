namespace Models.Response
{
    public class GetAccountResponse
    {
        public GetAccountResponse(Guid accountId, Currency currency)
        {
            AccountId = accountId;
            Currency = currency;
        }
        public Guid AccountId { get; set; }
        public Currency Currency { get; set; }

    }
}
