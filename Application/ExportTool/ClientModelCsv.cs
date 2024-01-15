namespace ExportTool
{
    public class ClientModelCsv
    {
        public ClientModelCsv(Guid passportId, string name, int age, string numberPhone, bool inBlackList)
        {
            PassportId = passportId;
            Name = name;
            Age = age;
            Phone = numberPhone;
            InBlackList = inBlackList;
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public bool InBlackList { get; set; }
        public Guid PassportId { get; set; }
    }
}
