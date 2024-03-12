using System.Net;
using System.Text;

namespace TinyGameServer.Utilities.HttpListenserWrapper
{
    public class HttpListenerResponseWrapper : IHttpListenerResponseWrapper
    {
        private readonly HttpListenerResponse _response;

        public HttpListenerResponseWrapper(HttpListenerResponse response)
        {
            _response = response;
        }

        public void Close() => _response.Close();
        public Stream OutputStream => _response.OutputStream;
        public long ContentLength64
        {
            get => _response.ContentLength64;
            set => _response.ContentLength64 = value;
        }
        public string ContentType
        { 
            get => _response.ContentType;
            set => _response.ContentType = value;
        }
        public Encoding ContentEncoding
        {
            get => _response.ContentEncoding;
            set => _response.ContentEncoding = value;
        }
        public int StatusCode
        {
            get => _response.StatusCode;
            set => _response.StatusCode = value;
        }
    }
}
