using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator
{
  public class PassGeneratorRequest
  {
    public string Identifier { get; set; }
    public int FormatVersion { get; set; }
    public string SerialNumber { get; set; }
    public string Description { get; set; }
  }
}
