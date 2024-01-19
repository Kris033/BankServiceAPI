using System.Text.Json.Serialization;

namespace Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum JobPosition
    {
        Trainee,
        Cashier,
        Security,
        Director
    }
}
