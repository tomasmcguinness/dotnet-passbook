using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Passbook.Sample.Web.Controllers
{
    public class PassRegistrationController : ApiController
    {
        //https://webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier/serialNumber
        public async Task<HttpResponseMessage> Post(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, HttpRequestMessage message)
        {
            string context = await message.Content.ReadAsStringAsync();
            //object content = await message.Content.ReadAsAsync<();
            JsonSerializer serializer = new JsonSerializer();
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        //https://webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier?passesUpdatedSince=tag
        public async Task<HttpResponseMessage> Get(string version, string deviceLibraryIdentifier, string passTypeIdentifier, HttpRequestMessage request)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
