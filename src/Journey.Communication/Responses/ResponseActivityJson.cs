using Journey.Communication.Enums;
using System.Text.Json.Serialization;

namespace Journey.Communication.Responses;
public class ResponseActivityJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActivityStatus Status { get; set; }
}
