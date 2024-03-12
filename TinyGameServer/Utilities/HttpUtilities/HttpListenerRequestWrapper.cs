using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace TinyGameServer.Utilities.HttpListenserWrapper
{
    public class HttpListenerRequestWrapper : IHttpListenerRequestWrapper
    {
        private readonly HttpListenerRequest _request;

        public HttpListenerRequestWrapper(HttpListenerRequest request)
        {
            _request = request;
        }

        public string[] AcceptTypes => _request.AcceptTypes;
        public long ContentLength64 => _request.ContentLength64;
        public Stream InputStream => _request.InputStream;
        public NameValueCollection Headers => _request.Headers;

        public Uri Url => _request.Url;

        public Encoding ContentEncoding => _request.ContentEncoding;

        public string HttpMethod => _request.HttpMethod;

        public string UserHostName => _request.UserHostName;

        public string UserAgent => _request.UserAgent;
    }
}
