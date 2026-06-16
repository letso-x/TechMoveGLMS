using System.Text.Json;
using System.Text.Json.Serialization;

namespace TechMoveGLMS.Test.Integration
{
    public static class TestJsonOptions
    {
        public static readonly JsonSerializerOptions Value = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        };
    }
}
