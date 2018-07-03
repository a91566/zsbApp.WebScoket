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
            var server = new Fleck.WebSocketServer("ws://0.0.0.0:9146");
            //var x = new Fleck.websocketc
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
                    foreach (var item in allSockets)
                    {
                        item.IWebSocketConnection.Send($"{x.User}下线了");
                    }
                };
                socket.OnMessage = message =>
                {
                    if (string.IsNullOrEmpty(message))
                        return;
                    var fromUser = allSockets.Where(i => i.ID == socket.ConnectionInfo.Id.ToString()).FirstOrDefault();
                    if (fromUser != null && string.IsNullOrEmpty(fromUser.User))
                    {
                        fromUser.User = message;
                        foreach (var item in allSockets)
                        {
                            item.IWebSocketConnection.Send($"{message}上线啦。");
                        }
                        return;
                    }
                    Console.WriteLine($"{socket.ConnectionInfo.Id}:{message}");
                    foreach (var item in allSockets)
                    {
                        item.IWebSocketConnection.Send($"{fromUser.User}:{message}");
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
