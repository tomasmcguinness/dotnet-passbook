using System;

namespace Passbook.Generator.Exceptions
{
    public class DuplicateFieldKeyException : Exception
    {
        public DuplicateFieldKeyException(string key) :
            base($"A field with the key `{key}` is already present")
        { }
    }
}
