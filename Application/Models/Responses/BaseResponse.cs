namespace Models.Responses
{
    public class BaseResponse
    {
        public bool IsSucsess { get; set; }
        public string ErrorMessage { get; set; } = "-";
    }
}
