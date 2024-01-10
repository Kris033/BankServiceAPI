namespace Models.Validations
{
    public static partial class AccountExtensions
    {
        public static void Validation(this Account account)
        {
            account.Client.Validation();
            account.BankAccount.Validation();
            string[] partsNumberBank = account.AccountNumber.Split(' ');
            if (partsNumberBank.Length != 4)
                throw new ArgumentOutOfRangeException("Номер банковского счета не поделен на 4 части и/или пропущенны знаки пробела");
            if (partsNumberBank.Any(n => n.Length != 4))
                throw new ArgumentOutOfRangeException("Номер банковского счета не соответствует размерам");
            if (partsNumberBank.Any(pn => pn.Any(n => !Char.IsDigit(n))))
                throw new ArgumentException("В номере банковского счета содержаться недопустимые символы");
        }
    }
}
