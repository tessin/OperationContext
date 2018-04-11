using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Tessin.Diagnostics.Internal
{
    internal class OperationValueKeyJsonConverter : JsonConverter
    {
        private static readonly JsonSerializer _serializer = new JsonSerializer { Converters = { new StringEnumConverter() } };

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
            var key = (OperationValueKey)value;
            var v = Enum.ToObject(key.KeyType, key.KeyValue);
            _serializer.Serialize(writer, v);
        }
    }
}
