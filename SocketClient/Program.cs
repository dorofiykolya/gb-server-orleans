using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zlib;

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
                    case "status":
                        if (_client != null && _client.Client.Poll(10, SelectMode.SelectWrite) && _client.Connected)
                        {
                            Console.WriteLine("socket connected");
                        }
                        else
                        {
                            Console.WriteLine("socket not connected");
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
                        Close();
                        break;
                    case "send":
                        Console.WriteLine("Enter message");
                        var message = Console.ReadLine();
                        Send(message);
                        Console.WriteLine("Message sent");
                        break;
                    case "auth":
                        Console.WriteLine("Enter developerId");
                        var developerId = Console.ReadLine();
                        Send("{\"authByDeveloper\":{\"developerId\":\"" + developerId + "\"}}");
                        Console.WriteLine("Auth sent");
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
                try
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    bytes = ZlibStream.CompressBuffer(bytes);
                    var len = BitConverter.GetBytes(bytes.Length);
                    Array.Reverse(len);
                    _client.GetStream().Write(len, 0, 4);
                    _client.GetStream().Write(bytes, 0, bytes.Length);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        static void ConnectTo(string host, int port)
        {
            if (_client != null)
            {
                _client.Close();
            }
            try
            {
                _client = new System.Net.Sockets.TcpClient();
                _client.Connect(host, port);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

            var thread = new Thread(() =>
            {
                var stream = _client.GetStream();
                var readLength = true;
                var memory = new MemoryStream();
                var reader = new BinaryReader(memory);
                var packetLength = 0;
                while (true)
                {
                    Thread.Sleep(100);
                    if (_client == null || !_client.Connected)
                    {
                        break;
                    }

                    var data = stream.DataAvailable;
                    if (data)
                    {
                        var buffer = new byte[_client.ReceiveBufferSize];
                        var count = stream.Read(buffer, 0, _client.ReceiveBufferSize);
                        memory.Write(buffer, 0, count);
                        memory.Position -= count;
                    }
                    if (memory.Position < memory.Length)
                    {
                        if (readLength)
                        {
                            var byteCount = memory.Length - memory.Position;
                            if (byteCount >= 4)
                            {
                                var len = reader.ReadBytes(4);
                                Array.Reverse(len);
                                packetLength = BitConverter.ToInt32(len, 0);
                                readLength = false;
                            }
                        }
                        else
                        {
                            if (packetLength <= memory.Length - memory.Position)
                            {
                                var packet = Ionic.Zlib.ZlibStream.UncompressBuffer(reader.ReadBytes(packetLength));
                                var message = Encoding.UTF8.GetString(packet, 0, packet.Length);
                                Console.WriteLine("MESSAGE:" + message);
                                readLength = true;
                            }
                        }
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
