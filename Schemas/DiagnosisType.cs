using System.Text.Json.Serialization;

namespace scrubsAPI
{
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum DiagnosisType
        {
        Main = 0,
        Concomitant,
        Complication
    }

}
