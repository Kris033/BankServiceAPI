namespace Models.Filters
{
    public class GetFilterRequest
    {
        public string? SearchFullName { get; set; }
        public string? SearchNumberPhone { get; set; }
        public string? SearchNumberPassport { get; set; }
        public DateOnly? DateBornFrom { get; set; }
        public DateOnly? DateBornTo { get; set; }
        public int? CountItem { get; set; }
    }
}
