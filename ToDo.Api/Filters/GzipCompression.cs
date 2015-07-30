using System.Net.Http;

using System.Web.Http.Filters;
using ToDo.Api.Helper;

namespace ToDo.Api.Filters
{
    public class GzipCompression : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var content = actionExecutedContext.Response.Content;
            byte[] contentAsByteArray;
            if (content == null)
            {
                contentAsByteArray = new byte[0];
            }
            contentAsByteArray = content.ReadAsByteArrayAsync().Result;
            contentAsByteArray = CompressionHelper.GzipCompression(contentAsByteArray);
            actionExecutedContext.Response.Content = new ByteArrayContent(contentAsByteArray);
            actionExecutedContext.Response.Content.Headers.Remove("Content-Type");
            actionExecutedContext.Response.Content.Headers.Add("Content-encoding", "gzip");
            actionExecutedContext.Response.Content.Headers.Add("Content-Type", "application/json");
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
