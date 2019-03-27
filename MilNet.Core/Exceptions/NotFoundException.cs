using System;
using System.Runtime.Serialization;

namespace MilNet.Core.Exceptions
{
    /// <summary>Not found exception</summary>
    [Serializable]
    public class NotFoundException : SystemException
    {
        /// <summary>Constructor</summary>
        public NotFoundException() : base() { }
        /// <summary>Constructor</summary>
        public NotFoundException(string message) : base(message) { }
        /// <summary>Constructor</summary>
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
        /// <summary>Constructor</summary>
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
