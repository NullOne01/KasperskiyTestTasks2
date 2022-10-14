using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using scan_service.Connection.Messages;
using scan_service.Extensions;
using scan_util.Input;

namespace scan_util
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(localEndPoint);

            ArgumentReader argumentReader = new ArgumentReader();

            Dictionary<string, Action> commands = new Dictionary<string, Action>()
            {
                {
                    "scan", () =>
                    {
                        DirectoryInfo directoryInfo = argumentReader.ReadDirectoryInfo(args, 1);
                        ScanCommand(directoryInfo, sender);
                    }
                },
                {
                    "status", () =>
                    {
                        int id = argumentReader.ReadInt(args, 1);
                        StatusCommand(id, sender);
                    }
                }
            };

            argumentReader.CheckCommand(args, 0, commands);

            byte[] gotRawMessage = Array.Empty<byte>();
            while (gotRawMessage.Length <= 0)
            {
                gotRawMessage = sender.ReceiveAll();
            }
            Message gotMessage = new Message() { Data = gotRawMessage };
            object responseObject = MessageSerializer.Deserialize(gotMessage);

            Console.WriteLine("Got message");

            if (responseObject is ScanResponse)
            {
                ScanResponse response = (ScanResponse)responseObject;
                Console.WriteLine($"Scan task was created with ID: {response.Id}");
            }

            if (responseObject is CheckStatusResponse)
            {
                CheckStatusResponse response = (CheckStatusResponse)responseObject;
                if (response.ScanResult != null)
                {
                    Console.WriteLine("====== Scan result ======");
                    Console.WriteLine(response.ScanResult);
                    Console.WriteLine("=========================");
                }
                else
                {
                    Console.WriteLine($"Scan task in progress, please wait");
                }
            }
        }

        private static void ScanCommand(DirectoryInfo directoryInfo, Socket socket)
        {
            ScanRequest request = new ScanRequest() { Path = directoryInfo.FullName };
            Message message = MessageSerializer.Serialize(request);
            socket.Send(message.Data);
        }

        private static void StatusCommand(int id, Socket socket)
        {
            CheckStatusRequest request = new CheckStatusRequest() { Id = id };
            Message message = MessageSerializer.Serialize(request);
            socket.Send(message.Data);
        }
    }
}