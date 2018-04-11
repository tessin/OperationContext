using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Tessin.Diagnostics.Internal
{
    [JsonConverter(typeof(OperationValueKeyJsonConverter))]
    public struct OperationValueKey : IEquatable<OperationValueKey>
    {
        public static OperationValueKey Create<TKey>(TKey key)
            where TKey : struct, IComparable, IFormattable, IConvertible
        {
            var keyType = typeof(TKey);
            if (!keyType.IsEnum)
            {
                throw new ArgumentException("type parameter TKey should be of type enum", nameof(TKey));
            }
            var keyValue = key.ToInt32(null);
            return new OperationValueKey(keyType, keyValue);
        }

        public readonly Type KeyType;
        public readonly int KeyValue;

        public OperationValueKey(Type keyType, int keyValue)
        {
            KeyType = keyType;
            KeyValue = keyValue;
        }

        public override int GetHashCode()
        {
            if (KeyType == null)
            {
                return 0;
            }
            return KeyValue;
        }

        public bool Equals(OperationValueKey other)
        {
            return (KeyType == other.KeyType) & (KeyValue == other.KeyValue);
        }

        public override string ToString()
        {
            return (string)JToken.FromObject(this);
        }
    }
}
