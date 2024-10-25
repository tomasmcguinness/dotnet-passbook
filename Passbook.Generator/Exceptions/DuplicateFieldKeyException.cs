using System;

namespace Passbook.Generator.Exceptions;

public class DuplicateFieldKeyException(string key) : Exception($"A field with the key `{key}` is already present")
{
}
