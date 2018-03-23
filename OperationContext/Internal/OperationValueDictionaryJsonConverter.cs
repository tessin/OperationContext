using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                writer.WritePropertyName(item.Key);
                item.Value.WriteJson(writer);
            }
            writer.WriteEndObject();
        }
    }
}
