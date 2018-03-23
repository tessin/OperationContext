using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Tessin.Diagnostics.Internal;

namespace Tessin.Diagnostics
{
    [JsonConverter(typeof(OperationValueJsonConverter))]
    public struct OperationValue
    {
        public static implicit operator OperationValue(DateTime value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(long value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(decimal value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(char value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(int value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(ulong value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(double value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(float value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(DateTimeOffset value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(bool value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(Guid value) => new OperationValue(new JValue(value));
        public static implicit operator OperationValue(TimeSpan value) => new OperationValue(new JValue(value));

        public static implicit operator OperationValue(DateTime? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(long? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(decimal? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(char? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(int? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(ulong? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(double? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(float? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(DateTimeOffset? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(bool? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(Guid? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;
        public static implicit operator OperationValue(TimeSpan? value) => value.HasValue ? new OperationValue(new JValue(value.Value)) : Null;

        public static implicit operator OperationValue(string value) => value != null ? new OperationValue(new JValue(value)) : Null;
        public static implicit operator OperationValue(Uri value) => value != null ? new OperationValue(new JValue(value)) : Null;

        public static explicit operator DateTime(OperationValue value) => (DateTime)value._value;
        public static explicit operator long(OperationValue value) => (long)value._value;
        public static explicit operator decimal(OperationValue value) => (decimal)value._value;
        public static explicit operator char(OperationValue value) => (char)value._value;
        public static explicit operator int(OperationValue value) => (int)value._value;
        public static explicit operator ulong(OperationValue value) => (ulong)value._value;
        public static explicit operator double(OperationValue value) => (double)value._value;
        public static explicit operator float(OperationValue value) => (float)value._value;
        public static explicit operator DateTimeOffset(OperationValue value) => (DateTimeOffset)value._value;
        public static explicit operator bool(OperationValue value) => (bool)value._value;
        public static explicit operator Guid(OperationValue value) => (Guid)value._value;
        public static explicit operator TimeSpan(OperationValue value) => (TimeSpan)value._value;

        public static explicit operator DateTime? (OperationValue value) => (DateTime?)value._value;
        public static explicit operator long? (OperationValue value) => (long?)value._value;
        public static explicit operator decimal? (OperationValue value) => (decimal?)value._value;
        public static explicit operator char? (OperationValue value) => (char?)value._value;
        public static explicit operator int? (OperationValue value) => (int?)value._value;
        public static explicit operator ulong? (OperationValue value) => (ulong?)value._value;
        public static explicit operator double? (OperationValue value) => (double?)value._value;
        public static explicit operator float? (OperationValue value) => (float?)value._value;
        public static explicit operator DateTimeOffset? (OperationValue value) => (DateTimeOffset?)value._value;
        public static explicit operator bool? (OperationValue value) => (bool?)value._value;
        public static explicit operator Guid? (OperationValue value) => (Guid?)value._value;
        public static explicit operator TimeSpan? (OperationValue value) => (TimeSpan?)value._value;

        public static explicit operator string(OperationValue value) => (string)value._value;
        public static explicit operator Uri(OperationValue value) => (Uri)value._value;

        public static readonly OperationValue Null = new OperationValue(JValue.CreateNull());

        public static OperationValue ReadJson(JsonReader reader)
        {
            return new OperationValue((JValue)JToken.ReadFrom(reader));
        }

        private readonly JValue _value;

        public bool IsEmpty => _value == null;
        public bool HasValue => _value != null;

        private OperationValue(JValue value)
        {
            _value = value;
        }

        public void WriteJson(JsonWriter writer)
        {
            _value.WriteTo(writer);
        }

        public override string ToString()
        {
            return _value?.ToString();
        }
    }
}
