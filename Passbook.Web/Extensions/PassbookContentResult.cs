using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Passbook.Web.Extensions
{
    public class PassbookContentResult : IHttpActionResult
    {
        private readonly byte[] mContent = null;
        private readonly DateTime? mLastModified = null;

        public PassbookContentResult(byte[] content)
        {
            mContent = content;
        }

        public PassbookContentResult(byte[] content, DateTime dateTime): this(content)
        {
            mLastModified = dateTime;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(mContent)
                    };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.apple.pkpass");
                response.Content.Headers.LastModified = mLastModified.HasValue ? mLastModified.Value : DateTime.UtcNow;
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                response.Content.Headers.ContentDisposition.CreationDate = mLastModified;
                response.Content.Headers.ContentDisposition.FileName = "pass.pkpass";
                response.Content.Headers.ContentDisposition.Name = "pass.pkpass";

                return response;
            }, cancellationToken);
        }
    }
}

