using System;
using System.Net.Sockets;
using Utils;

namespace SocketProxy
{
    public class SocketServer : IDisposable
    {
        public event Action<Socket> Socket;

        private Socket _socket;

        public SocketServer()
        {

        }

        public void Dispose()
        {
            DestroySocket();
        }

        public void Start(int port)
        {
            CreateSocket();
            StartListen(port);
        }

        private void StartListen(int port)
        {
            for (var i = 0; i < 5; i++)
            {
                _socket.BeginAccept(OnSocketAccept, _socket);
            }
            _socket.Listen(1000);
        }

        private void OnSocketAccept(IAsyncResult asyncResult)
        {
            var socket = ((Socket)(asyncResult.AsyncState));
            var client = socket.EndAccept(asyncResult);
            if (Socket != null)
            {
                Socket(client);
            }
        }

        private void CreateSocket()
        {
            DestroySocket();
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        private void DestroySocket()
        {
            if (_socket != null)
            {
                SafeInvoke.Invoke(_socket.Shutdown, SocketShutdown.Both);
                SafeInvoke.Invoke(_socket.Close);
                SafeInvoke.Invoke(_socket.Dispose);
                _socket = null;
            }
        }
    }
}
