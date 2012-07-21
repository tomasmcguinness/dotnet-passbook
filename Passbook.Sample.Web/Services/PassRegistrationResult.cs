using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passbook.Sample.Web.Services
{
  public enum PassRegistrationResult
  {
    Failed,
    SuccessfullyRegistered,
    AlreadyRegistered,
    UnAuthorized
  }
}