namespace Models.Response
{
    public class GetExChangeResponse
    {
        public int Error { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
