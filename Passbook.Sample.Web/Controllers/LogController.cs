using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Passbook.Sample.Web.Controllers
{
    public class LogController : ApiController
    {
        public async Task<HttpResponseMessage> Post(string version, HttpRequestMessage message)
        {
            string jsonBody = await message.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(jsonBody);
            string log = json["log"].Value<string>();

            Debug.WriteLine(log);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
