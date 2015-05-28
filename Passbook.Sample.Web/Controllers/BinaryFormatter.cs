using System;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Net;

namespace Passbook.Sample.Web.Controllers
{
    class BinaryFormatter : MediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Byte[]);
        }

        public override Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext, System.Threading.CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() =>
                {
                    Byte[] array = value as Byte[];
                    writeStream.Write(array, 0, array.Length);
                    writeStream.Flush();
                });

            return task;
        }
    }
}
