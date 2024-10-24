using System.Text.Json.Serialization;

namespace scrubsAPI
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male = 0,
        Female
    }
}
