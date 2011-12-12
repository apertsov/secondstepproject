using System;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;
using System.Web;

namespace DistanceLessons.Attributes
{
    public class EnableCompressionAttribute : ActionFilterAttribute
    {
        const CompressionMode compress = CompressionMode.Compress;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            HttpResponseBase responce = filterContext.HttpContext.Response;
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (acceptEncoding == null)
                return;
            if (acceptEncoding.ToLower().Contains("qzip"))
            {
                responce.Filter = new GZipStream(responce.Filter, compress);
                responce.AppendHeader("Content-Enciding", "qzip");
            }
            else
                if (acceptEncoding.ToLower().Contains("deflate"))
                {
                    responce.Filter = new DeflateStream(responce.Filter, compress);
                    responce.AppendHeader("Content-Encoding", "deflate");
                }

        }
    }
}