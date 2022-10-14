using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using scan_service.Connection.Messages;
using scan_service.Data;
using scan_service.Extensions;
using scan_service.Scanner;

namespace scan_service.Connection
{
    public class ServiceConnectionListener
    {
        private ServiceThread _serviceThread = new ServiceThread();
        private Socket _listener;

        public ServiceConnectionListener(Socket listener)
        {
            _listener = listener;
        }

        public void Start()
        {
            Thread thread = new Thread(_serviceThread.Start);
            thread.Start();
            
            while (true)
            {
                Socket clientSocket = _listener.Accept();
                byte[] gotRawMessage = Array.Empty<byte>();
                while (gotRawMessage.Length <= 0)
                {
                    gotRawMessage = clientSocket.ReceiveAll();
                }
                // byte[] gotRawMessage = clientSocket.ReceiveAll();
                Message gotMessage = new Message() { Data = gotRawMessage };
                CheckMessage(MessageSerializer.Deserialize(gotMessage), clientSocket);
            }
        }

        private void CheckMessage(object receivedObject, Socket clientSocket)
        {
            Console.WriteLine("Got message");
            if (receivedObject is CheckStatusRequest)
            {
                CheckStatusRequest checkStatusRequest = (CheckStatusRequest)receivedObject;
                var result = _serviceThread.GetResult(checkStatusRequest.Id);
                CheckStatusResponse response = new CheckStatusResponse() { ScanResult = result };
                SendMessage(MessageSerializer.Serialize(response), clientSocket);
            }

            if (receivedObject is ScanRequest)
            {
                ScanRequest scanRequest = (ScanRequest)receivedObject;
                int id = _serviceThread.EnqueueScanTask(scanRequest.Path);
                ScanResponse response = new ScanResponse() { Id = id };
                SendMessage(MessageSerializer.Serialize(response), clientSocket);
            }
        }

        private void SendMessage(Message message, Socket clientSocket)
        {
            clientSocket.Send(message.Data);
        }
    }
}