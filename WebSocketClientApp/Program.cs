using System;
using System.Net.WebSockets;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketClientApp
{
    
    internal class Program
    {
        
        static void Main(string[] args)
        {
            using (WebSocketSharp.WebSocket ws = new WebSocketSharp.WebSocket("ws://127.0.0.1:7890/EchoAll"))
            {
                ws.OnMessage += Ws_OnMessage;

                ws.Connect();
                ws.Send("Hello from PCamp!");

                string mesaj = Console.ReadLine();

                ws.Send(mesaj);

                Console.ReadKey();
            }
        }
        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Received from the server: " + e.Data);
        }
    }
}
