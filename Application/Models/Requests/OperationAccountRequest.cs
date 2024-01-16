using Models.Enums;

namespace Models.Requests
{
    public class OperationAccountRequest
    {
        public OperationAccountRequest(
            Guid accountId,
            TypeOperationAccount operationAccount,
            Currency? currency = null) 
        {
            AccountId = accountId;
            OperationAccount = operationAccount;
            Currency = currency;
        }
        public Guid AccountId { get; set; }
        public TypeOperationAccount OperationAccount { get; set; }
        public Currency? Currency { get; set; }
    }
}
