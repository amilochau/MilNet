using System;

namespace MilNet.Core.Types
{
    /// <summary>Interface of readable/writable enumeration</summary>
    public interface IReadableEnum<TClasseEnum, TTypeEnum> : IEquatable<TClasseEnum> where TClasseEnum : class where TTypeEnum : struct
    {
        /// <summary>Detail as value</summary>
        TTypeEnum Value { get; set; }

        /// <summary>Detail as text</summary>
        string Text { get; set; }

        /// <summary>Detail as text</summary>
        /// <remarks>the ToString() method must be overrided</remarks>
        string ToString();
        /// <summary>Read as string</summary>
        void FromString(string text);
    }
}
