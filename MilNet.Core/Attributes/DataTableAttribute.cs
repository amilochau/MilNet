using System;

namespace MilNet.Core.Attributes
{
    /// <summary>Data table property</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DataTableAttribute : LocalizedAttribute
    {
        /// <summary>Constructor</summary>
        public DataTableAttribute() : base() { }

        /// <summary>Display format</summary>
        public string Format { get; set; }

        /// <summary>Header string (localized or not)</summary>
        public string Header => MessageString;
    }
}
