using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        private static System.Net.Sockets.TcpClient _client;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter command");
                var command = Console.ReadLine();
                switch (command)
                {
                    case "local":
                        {
                            Console.WriteLine("Enter PORT");
                            var target = Console.ReadLine();
                            var port = int.Parse(target);
                            ConnectTo("127.0.0.1", port);
                        }
                        break;
                    case "connect":
                        {
                            Console.WriteLine("Enter HOST:PORT");
                            var target = Console.ReadLine();
                            var array = target.Split(':');
                            var host = array[0];
                            var port = int.Parse(array[1]);
                            ConnectTo(host, port);
                        }
                        break;
                    case "close":
                        break;
                    case "send":
                        Console.WriteLine("Enter message");
                        var message = Console.ReadLine();
                        Send(message);
                        Console.WriteLine("Message sent");
                        break;
                    case "exit":
                    case "quit":
                        Close();
                        return;
                }
            }
        }

        static void Send(string message)
        {
            if (_client != null && _client.Connected)
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                _client.GetStream().Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                _client.GetStream().Write(bytes, 0, bytes.Length);
            }
        }

        static void ConnectTo(string host, int port)
        {
            if (_client != null)
            {
                _client.Close();
            }

            _client = new System.Net.Sockets.TcpClient();
            _client.Connect(host, port);
            var thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    var stream = _client.GetStream();
                    var data = stream.DataAvailable;
                    if (data)
                    {
                        var buffer = new byte[stream.Length - stream.Position];
                        var count = stream.Read(buffer, 0, buffer.Length);
                        var message = Encoding.UTF8.GetString(buffer, 0, count);
                        Console.WriteLine("MESSAGE:" + message);
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        static void Close()
        {
            if (_client != null)
            {
                _client.Close();
                _client = null;
            }
        }
    }
}
