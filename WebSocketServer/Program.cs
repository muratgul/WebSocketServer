using Microsoft.Win32;
using System;
using System.Net;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketServerApp
{

    internal class Program
    {
        static void Main(string[] args)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (ReadSettings.FromConfig("autostart") == "true")
            {
                rk.SetValue("VWDWebSocket", System.Reflection.Assembly.GetExecutingAssembly().Location.ToString());
            }
            else
            {
                rk.DeleteValue("VWDWebSocket", false);
            }

            Console.Title = "VWD WebSocket Server 0.2";

            string url = $@"ws://{ReadSettings.FromConfig("ipadress")}:{ReadSettings.FromConfig("port")}";

            try
            {
                WebSocketServer wssv = new WebSocketServer(url);

                wssv.AddWebSocketService<Entegrasyon>("/Entegrasyon");
                wssv.AddWebSocketService<HR>("/HR");
                wssv.AddWebSocketService<Sevkiyat>("/Sevkiyat");

                wssv.Start();

                Console.WriteLine($"WS server started on {url}/Entegrasyon");
                Console.WriteLine($"WS server started on {url}/HR");
                Console.WriteLine($"WS server started on {url}/Sevkiyat");

                Console.ReadKey();

                string command = Console.ReadLine();

                if (command == "exit")
                {
                    wssv.Stop();
                }

                wssv.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            
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
    public class Sevkiyat : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Sevkiyat client: " + e.Data);
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
