using System;
using System.Globalization;
using System.Text.Json;

namespace Neo.IO.Json
{
    public class SJString : SJObject
    {
        public string Value { get; private set; }

        public SJString(string value)
        {
            this.Value = value ?? throw new ArgumentNullException();
        }

        public override bool AsBoolean()
        {
            return !string.IsNullOrEmpty(Value);
        }

        public override double AsNumber()
        {
            if (string.IsNullOrEmpty(Value)) return 0;
            return double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result) ? result : double.NaN;
        }

        public override string AsString()
        {
            return Value;
        }

        public override T TryGetEnum<T>(T defaultValue = default, bool ignoreCase = false)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), Value, ignoreCase);
            }
            catch
            {
                return defaultValue;
            }
        }

        internal override void Write(Utf8JsonWriter writer)
        {
            writer.WriteStringValue(Value);
        }

        public override SJObject Clone()
        {
            return this;
        }

        public static implicit operator SJString(Enum value)
        {
            return new SJString(value.ToString());
        }

        public static implicit operator SJString(string value)
        {
            return value == null ? null : new SJString(value);
        }
    }
}
