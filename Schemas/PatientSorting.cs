using System.Text.Json.Serialization;

namespace scrubsAPI.Schemas
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PatientSorting
    {
        NameAsc = 0,
        NameDesc,
        CreateAsc, 
        CreateDesc,
        InspectionAsc,
        InspectionDesc
    }
}
