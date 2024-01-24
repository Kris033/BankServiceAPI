namespace Models.Exports
{
    public class ClientExportModel
    {
        public ClientExportModel(
            Guid passportId,
            string name,
            int age,
            string numberPhone,
            bool inBlackList,
            List<AccountExportModel> accounts)
        {
            PassportId = passportId;
            Name = name;
            Age = age;
            Phone = numberPhone;
            InBlackList = inBlackList;
            Accounts = accounts;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public bool InBlackList { get; set; }
        public Guid PassportId { get; set; }
        public List<AccountExportModel> Accounts { get; set; }
    }
}
