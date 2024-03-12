using System.Text;

namespace TinyGameServer.Utilities.HttpListenserWrapper
{
    public interface IHttpListenerResponseWrapper
    {
        void Close();
        Stream OutputStream { get; }
        long ContentLength64 { get; set; }
        string ContentType { get; set; }
        Encoding ContentEncoding { get; set; }
        int StatusCode { get; set; }

    }
}
