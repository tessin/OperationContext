using Newtonsoft.Json;
using System;

namespace Tessin.Diagnostics.Internal
{
    internal class OperationValueDictionaryJsonConverter : JsonConverter
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
            writer.WriteStartObject();
            foreach (var item in (OperationValueDictionary)value)
            {
                writer.WritePropertyName(item.Key.ToString());
                serializer.Serialize(writer, item.Value);
            }
            writer.WriteEndObject();
        }
    }
}
