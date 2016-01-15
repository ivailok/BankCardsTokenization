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
    public class Client
    {
        private const string LocalHost = "127.0.0.1";
        private const int Port = 44500;

        private readonly TcpClient TcpClient;
        private readonly Thread ConnectionThread;
        private readonly NetworkStream SocketStream;
        private readonly BinaryReader Reader;
        private readonly BinaryWriter Writer;
        private readonly BinaryFormatter Formatter;

        public Client()
        {
            this.TcpClient = new TcpClient(LocalHost, Port);
            this.SocketStream = this.TcpClient.GetStream();
            this.Reader = new BinaryReader(this.SocketStream);
            this.Writer = new BinaryWriter(this.SocketStream);
            this.Formatter = new BinaryFormatter();
            this.ConnectionThread = new Thread(new ThreadStart(this.Receive));
            this.ConnectionThread.Start();
        }

        public void Receive()
        {

        }

        public void Send(Package package)
        {
            this.Formatter.Serialize(this.SocketStream, package);
        }
    }
}
