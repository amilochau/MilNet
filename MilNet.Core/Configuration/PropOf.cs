using System;
using System.ComponentModel;

namespace MilNet.Core.Configuration
{
    /// <summary>Typed configuration property</summary>
    /// <typeparam name="T">Type of the configuration property</typeparam>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public sealed class Prop<T> : Prop
    {
        /// <summary>Default value</summary>
        public T DefaultValue { get; private set; }

        private T innerValue;
        /// <summary>Value</summary>
        public T Value
        {
            get => innerValue;
            set
            {
                T oldValue = innerValue;
                innerValue = value;
                if (!innerValue.Equals(oldValue))
                    OnPropertyChanged();
            }
        }

        /// <summary>Constructor</summary>
        /// <param name="name">Name of the configuration key</param>
        /// <param name="defaultValue">Default value</param>
        public Prop(string name, T defaultValue) : base(name) => DefaultValue = defaultValue;

        /// <summary>Key value as string</summary>
        public override string ToString() => Value.ToString();

        /// <summary>Key value from string</summary>
        /// <exception cref="NotSupportedException"/>
        public override void FromString(string text)
        {
            T oldValue = Value;
            Value = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(text);
            if (!Value.Equals(oldValue))
                OnPropertyChanged();
        }
        /// <summary>Fill the value with the default value</summary>
        public override void UseDefaultValue() => Value = DefaultValue;
    }
}
