using Newtonsoft.Json;
using System;

namespace Tessin.Diagnostics.Internal
{
    internal class OperationContextJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, OperationContext.Backtrace(((OperationContext)value)));
        }
    }
}
