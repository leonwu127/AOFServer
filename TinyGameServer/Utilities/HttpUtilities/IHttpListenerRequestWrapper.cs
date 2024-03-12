using System.Collections.Specialized;
using System.Text;

namespace TinyGameServer.Utilities.HttpListenserWrapper
{
    public interface IHttpListenerRequestWrapper
    {
        string[] AcceptTypes { get; }
        long ContentLength64 { get; }
        Stream InputStream { get; }
        NameValueCollection Headers { get; }
        Uri Url { get; }
        Encoding ContentEncoding { get; }
        string HttpMethod { get; }
        string UserHostName { get; }
        string UserAgent { get; }
    }
}
