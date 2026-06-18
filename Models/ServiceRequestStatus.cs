using System.Text.Json.Serialization;
namespace TechMoveGLMS.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceRequestStatus
    {
        Draft,
        Active,
        Expired,
        OnHold
    }
}
