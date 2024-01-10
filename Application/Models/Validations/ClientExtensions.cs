namespace Models.Validations
{
    public static partial class ClientExtensions
    {
        public static void Validation(this Client client)
        {
            client.ValidationPerson();
        }
    }
}
