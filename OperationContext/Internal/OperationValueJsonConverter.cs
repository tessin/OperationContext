using Newtonsoft.Json;
using System;

namespace Tessin.Diagnostics.Internal
{
    internal class OperationValueJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return OperationValue.ReadJson(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ((OperationValue)value).WriteJson(writer);
        }
    }
}
