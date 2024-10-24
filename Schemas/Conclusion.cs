using System.Text.Json.Serialization;

namespace scrubsAPI
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Conclusion
    {
        Disease = 0,
        Recovery,
        Death
    }
}
