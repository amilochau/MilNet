using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace MilNet.Core.Types
{
    /// <summary>Readable/writable enumeration</summary>
    public abstract class ReadableEnum<TTypeEnum> : IReadableEnum<ReadableEnum<TTypeEnum>, TTypeEnum> where TTypeEnum : struct
    {
        /// <summary>Constructor</summary>
        /// <remarks>Fill <see cref="DefaultValue"/> if needed</remarks>
        protected ReadableEnum() { }

        /// <summary>Constructor</summary>
        /// <param name="defaultValue">Default value</param>
        /// <remarks><see cref="Value"/> will be filled with the default value; also if no value can be read from string</remarks>
        protected ReadableEnum(TTypeEnum defaultValue)
        {
            DefaultValue = defaultValue;
            Value = defaultValue;
        }

        /// <summary>Value</summary>
        public TTypeEnum Value { get; set; }
        /// <summary>Default value</summary>
        public TTypeEnum DefaultValue { get; }

        /// <summary>Value as string</summary>
        [NotMapped]
        public string Text
        {
            get { return ToString(); }
            set { FromString(value); }
        }
        
        /// <summary>Convert to string</summary>
        public abstract override string ToString();
        /// <summary>Read as string</summary>
        public virtual void FromString(string text)
        {
            // DefaultValue by default
            if (string.IsNullOrEmpty(text))
            {
                Value = DefaultValue;
                return;
            }

            // Try to understand text as a Text
            foreach (TTypeEnum type in Enum.GetValues(typeof(TTypeEnum)).Cast<TTypeEnum>())
            {
                Value = type;
                if (text.Equals(Text, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            // Try to understand text as a Value
            foreach (TTypeEnum type in Enum.GetValues(typeof(TTypeEnum)).Cast<TTypeEnum>())
            {
                if (text.Equals(string.Format(CultureInfo.InvariantCulture, "{0:d}", type), StringComparison.OrdinalIgnoreCase)
                    || text.Equals(string.Format(CultureInfo.InvariantCulture, "{0:g}", type), StringComparison.OrdinalIgnoreCase))
                {
                    Value = type;
                    return;
                }
            }

            // DefaultValue by default
            Value = DefaultValue;
        }
        
        /// <summary>Equality check</summary>
        public bool Equals(ReadableEnum<TTypeEnum> other) => other == null ? false : other.Value.Equals(Value);
    }
}
