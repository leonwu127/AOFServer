using System.Text;

namespace ArmyServer.Utilities.HttpListenserWrapper
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
