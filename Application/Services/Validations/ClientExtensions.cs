using Models;

namespace Services.Validations
{
    public static partial class ClientExtensions
    {
        public static void Validation(this Client client)
        {
            client.ValidationPerson();
        }
    }
}
