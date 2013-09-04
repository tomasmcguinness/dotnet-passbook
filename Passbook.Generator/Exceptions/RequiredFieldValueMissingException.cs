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
        private string key;

        public RequiredFieldValueMissingException(string fieldName) : base("Missing value for field with key '" + fieldName + "'. Every field must have a value specified.") { }

        public override string Message
        {
            get
            {
                return string.Format("Missing value for field [key: '{0}']. Every field must have a value specified.", key);
            }
        }
    }
}
