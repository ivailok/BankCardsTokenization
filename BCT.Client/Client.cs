using BCT.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCT.Client
{
    public class Client : IDisposable
    {
        private const string LocalHost = "127.0.0.1";
        private const int Port = 44500;

        private readonly TcpClient TcpClient;
        private readonly NetworkStream SocketStream;
        private readonly BinaryFormatter Formatter;

        public Client()
        {
            this.TcpClient = new TcpClient(LocalHost, Port);
            this.SocketStream = this.TcpClient.GetStream();
            this.Formatter = new BinaryFormatter();
        }

        public Task<Response> SendAsync(object data, RequestType type)
        {
            Request req = new Request()
            {
                Data = data,
                RequestType = type
            };

            return Task.Run(() =>
            {
                this.Formatter.Serialize(this.SocketStream, req);
                var response = this.Formatter.Deserialize(this.SocketStream) as Response;
                return response;
            });
        }

        public void Dispose()
        {
            this.SocketStream.Dispose();
        }
    }
}
