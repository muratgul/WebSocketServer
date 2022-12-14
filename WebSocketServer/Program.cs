using Microsoft.Win32;
using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketServerApp
{

    internal class Program
    {
        static void Main(string[] args)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (ReadSettings.FromConfig("autostart") == "true")
            {
                rk.SetValue("VWDWebSocket", System.Reflection.Assembly.GetExecutingAssembly().Location.ToString());
            }
            else
            {
                rk.DeleteValue("VWDWebSocket", false);
            }

            Console.Title = "VWD WebSocket Server";

            string url = $@"ws://{ReadSettings.FromConfig("ipadress")}:{ReadSettings.FromConfig("port")}";

            try
            {
                WebSocketServer wssv = new WebSocketServer(url);

                //wssv.AddWebSocketService<Echo>("/Echo");
                wssv.AddWebSocketService<Entegrasyon>("/Entegrasyon");
                //wssv.AddWebSocketService<HR>("/HR");

                wssv.Start();
                //Console.WriteLine($"WS server started on {url}/Echo");
                Console.WriteLine($"WS server started on {url}/Entegrasyon");
                //Console.WriteLine($"WS server started on {url}/HR");

                Console.ReadKey();

                string command = Console.ReadLine();

                if (command == "exit")
                {
                    wssv.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            } 
        }
    }
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received message from Echo client: " + e.Data);
            Send(e.Data);
        }
    }
    public class Entegrasyon : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Entegrasyon client: " + e.Data);
            Sessions.Broadcast(e.Data);
        }
    }

    public class HR : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("HR client: " + e.Data);
            Sessions.Broadcast(e.Data);
        }
    }
}
