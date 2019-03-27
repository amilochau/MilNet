using System.Collections.Generic;

namespace MilNet.Core.Types
{
    /// <summary>Bit field enumeration</summary>
    public abstract class BitFieldEnum<TTypeEnum> : IBitFieldEnum<BitFieldEnum<TTypeEnum>, TTypeEnum>
    {
        /// <summary>Value</summary>
        public TTypeEnum Value { get; set; }
        /// <summary>Default value</summary>
        public TTypeEnum DefaultValue { get; }
        
        /// <summary>Value as array</summary>
        public abstract IEnumerable<TTypeEnum> Values {get;set;}

        /// <summary>Convert to string</summary>
        public abstract override string ToString();

        /// <summary>Equality check</summary>
        public bool Equals(BitFieldEnum<TTypeEnum> other) => other == null ? false : other.Value.Equals(Value);
    }
}
