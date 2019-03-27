using System;
using System.Runtime.Serialization;

namespace MilNet.Core.Exceptions
{
    /// <summary>Forbidden exception</summary>
    [Serializable]
    public class ForbiddenException : SystemException
    {
        /// <summary>Constructor</summary>
        public ForbiddenException() : base() { }
        /// <summary>Constructor</summary>
        public ForbiddenException(string message) : base(message) { }
        /// <summary>Constructor</summary>
        public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
        /// <summary>Constructor</summary>
        protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
