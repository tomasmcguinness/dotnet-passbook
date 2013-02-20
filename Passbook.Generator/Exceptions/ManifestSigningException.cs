using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator.Exceptions
{
    [Serializable]
    public class ManifestSigningException : Exception
    {
        public ManifestSigningException() { }
        public ManifestSigningException(string message) : base(message) { }
        public ManifestSigningException(string message, Exception inner) : base(message, inner) { }
        protected ManifestSigningException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
