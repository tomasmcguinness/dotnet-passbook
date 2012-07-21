using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Sample.Web.Controllers
{
    class BinaryFormatter : MediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Byte[]);
        }

        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(Type type, System.IO.Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders, IFormatterLogger formatterLogger)
        {
            throw new NotImplementedException();
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, System.IO.Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders, System.Net.TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
                {
                    var array = value as byte[];
                    stream.Write(array, 0, array.Length);
                    stream.Flush();
                });

            return task;
        }
    }
}
