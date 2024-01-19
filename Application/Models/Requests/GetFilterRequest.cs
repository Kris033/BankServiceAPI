namespace Models.Requests
{
    public class GetFilterRequest
    {
        public string? SearchFullName { get; set; }
        public string? SearchNumberPhone { get; set; }
        public string? SearchNumberPassport { get; set; }
        public DateTime? DateBornFrom { get; set; }
        public DateTime? DateBornTo { get; set; }
        public int? CountItem { get; set; }
    }
}
