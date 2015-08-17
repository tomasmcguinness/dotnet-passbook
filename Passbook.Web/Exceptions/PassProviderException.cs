using System;

namespace Passbook.Web.Exceptions
{
    [Serializable]
    public class PassProviderException : Exception
    {
        public PassProviderException() { }
        public PassProviderException(string message) : base(message) { }
        public PassProviderException(string message, Exception inner) : base(message, inner) { }
        protected PassProviderException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

