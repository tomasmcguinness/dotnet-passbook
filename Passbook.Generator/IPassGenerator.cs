using System.Collections.Generic;

namespace Passbook.Generator;

public interface IPassGenerator
{
    /// <summary>
    /// Creates a byte array which contains one pkpass file
    /// </summary>
    /// <param name="generatorRequest">
    /// An instance of a PassGeneratorRequest</param>
    /// <returns>
    /// A byte array which contains a zipped pkpass file.
    /// </returns>
    public byte[] Generate(PassGeneratorRequest generatorRequest);

    /// <summary>
    /// Creates a byte array that can contains a .pkpasses file for bundling multiple passes together
    /// </summary>
    /// <param name="generatorRequests">
    /// A list of PassGeneratorRequest objects
    /// </param>
    /// <returns>
    /// A byte array which contains a zipped pkpasses file.
    /// </returns>
    public byte[] Generate(IReadOnlyList<PassGeneratorRequest> generatorRequests);
}
