﻿using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketServerApp
{
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received message from Echo client: " + e.Data);
            Send(e.Data);
        }
    }
    public class EchoAll : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received message from EchoAll client: " + e.Data);
            Sessions.Broadcast(e.Data);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            WebSocketServer wssv = new WebSocketServer("ws://192.168.0.6:7890");

            wssv.AddWebSocketService<Echo>("/Echo");
            wssv.AddWebSocketService<EchoAll>("/EchoAll");

            wssv.Start();
            Console.WriteLine("WS server started on ws://192.168.0.6:7890/Echo");
            Console.WriteLine("WS server started on ws://192.168.0.6:7890/EchoAll");

            Console.ReadKey();
            wssv.Stop();
        }
    }
}