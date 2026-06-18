using System.Text.Json.Serialization;
namespace TechMoveGLMS.Models

{

[JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ContractStatus
        {
            Draft,
            Active,
            Expired,
            OnHold
        }
    
}
