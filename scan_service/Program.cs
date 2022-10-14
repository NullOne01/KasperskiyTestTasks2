using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using scan_service.Connection;

namespace scan_service
{
    class Program
    {
        private static void Main(string[] args)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);
            
            ServiceConnectionListener serviceThread = new ServiceConnectionListener(listener);
            Thread thread = new Thread(serviceThread.Start);
            
            Console.WriteLine("Press ENTER to exit...");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            
            listener.Close();
            thread.Join();
        }
    }
}