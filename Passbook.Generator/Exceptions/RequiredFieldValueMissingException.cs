using System;

namespace Passbook.Generator.Exceptions;

[Serializable]
public class RequiredFieldValueMissingException(string fieldName) : Exception("Missing value for field [key: '" + fieldName + "']. Every field must have a value specified in the template or the individual pass.")
{
}
