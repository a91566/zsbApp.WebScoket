using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace zsbApp.WebScoket.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<SocketInfo>();
            var server = new WebSocketServer("ws://0.0.0.0:9146");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("连接打开!");
                    var x = allSockets.Where(i => i.ID == socket.ConnectionInfo.Id.ToString()).FirstOrDefault();
                    if (x != null)
                        return;
                    allSockets.Add(new SocketInfo() {
                        ID = socket.ConnectionInfo.Id.ToString(),
                        IWebSocketConnection = socket
                    });
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("连接关闭!");
                    var x = allSockets.Where(i=>i.ID == socket.ConnectionInfo.Id.ToString()).FirstOrDefault();
                    if(x!=null)
                        allSockets.Remove(x);
                };
                socket.OnMessage = message =>
                {
                    var x = allSockets.Where(i => i.ID == socket.ConnectionInfo.Id.ToString()).FirstOrDefault();
                    if (x!=null && string.IsNullOrEmpty(x.User))
                    {
                        x.User = message;
                        return;
                    }
                    Console.WriteLine($"{socket.ConnectionInfo.Id}:{message}");
                    foreach (var item in allSockets)
                    {
                        item.IWebSocketConnection.Send($"{item.User}:{message}");
                    }
                    //allSockets.Select(i=>i.IWebSocketConnection).ToList().ForEach(s => s.Send($"{s.us}:{message} "));
                };
            });


            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var socket in allSockets.Select(i=>i.IWebSocketConnection).ToList())
                {
                    socket.Send(input);
                }
                input = Console.ReadLine();
            }
        }
    }
        
    
}
