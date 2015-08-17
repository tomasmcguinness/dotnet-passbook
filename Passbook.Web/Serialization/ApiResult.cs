using System;

namespace Passbook.Web.Serialization
{
    public class ApiResult
    {
        public string message
        {
            get;
            set;
        }

        public ApiResult(string message)
        {
            this.message = message;
        }
    }
}

