using System;
using System.ComponentModel;

namespace MilNet.Core.Configuration
{
    /// <summary>Configuration property</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public abstract class Prop : IEquatable<Prop>, INotifyPropertyChanged
    {
        /// <summary>Notification of property value change</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Name of the configuration key</summary>
        public string Name { get; private set; }

        /// <summary>Constructor</summary>
        /// <param name="name">Name of the configuration key</param>
        /// <exception cref="ArgumentNullException"/>
        protected Prop(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        /// <summary>Test of equality between two configuration keys</summary>
        /// <remarks>Egality by <see cref="Name"/></remarks>
        public bool Equals(Prop other) => other == null ? false : Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

        /// <summary>Invoke a new <see cref="PropertyChangedEventHandler"/> if the property value changes</summary>
        protected void OnPropertyChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));

        /// <summary>Key value as string</summary>
        public abstract override string ToString();
        /// <summary>Key value from string</summary>
        /// <exception cref="NotSupportedException"/>
        public abstract void FromString(string text);
        /// <summary>Fill the value with the default value</summary>
        public abstract void UseDefaultValue();
    }
}
