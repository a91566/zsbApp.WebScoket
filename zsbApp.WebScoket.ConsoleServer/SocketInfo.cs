using Fleck;

namespace zsbApp.WebScoket.ConsoleServer
{
    public class SocketInfo
    {
        public string ID { get; set; }
        public string User { get; set; }
        public IWebSocketConnection IWebSocketConnection { get; set; }
    }
}
