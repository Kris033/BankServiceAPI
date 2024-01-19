using System.Text.Json.Serialization;

namespace Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypeOperationAccount
    {
        Put, //Положить на счёт
        Withdraw, //Снять со счёта
        CheckBalance
    }
}
