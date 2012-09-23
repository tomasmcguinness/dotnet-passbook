using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator.Exceptions
{
    [Serializable]
    public class RequiredFieldValueMissingException : Exception
    {
        public RequiredFieldValueMissingException(string fieldName) : base("Missing key value. Every Field must have a key specified.") { }
    }
}
