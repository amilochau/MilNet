using System;
using System.Collections.Generic;

namespace MilNet.Core.Types
{
    /// <summary>Interface of bit field enumeration</summary>
    public interface IBitFieldEnum<TClasseEnum, TTypeEnum> : IEquatable<TClasseEnum>
    {
        /// <summary>Detail as values</summary>
        TTypeEnum Value { get; set; }

        /// <summary>Detail as values array</summary>
        IEnumerable<TTypeEnum> Values { get; set; }
        
        /// <summary>Detail as text</summary>
        /// <remarks>the ToString() method must be overrided</remarks>
        string ToString();
    }
}
