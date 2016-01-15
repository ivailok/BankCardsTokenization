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

namespace BCT.Server
{
    public class Request
    {
        private readonly Socket RequestSocket;
        private readonly NetworkStream SocketStream;
        private readonly BinaryReader Reader;
        private readonly BinaryWriter Writer;
        private readonly BinaryFormatter Formatter;

        public Request(Socket requestSocket)
        {
            this.RequestSocket = requestSocket;
            this.SocketStream = new NetworkStream(this.RequestSocket);
            this.Reader = new BinaryReader(this.SocketStream);
            this.Writer = new BinaryWriter(this.SocketStream);
            this.Formatter = new BinaryFormatter();
        }

        public void Execute()
        {
            while (true)
            {
                while (this.RequestSocket.Available == 0)
                {
                    Thread.Sleep(1000);
                }

                var readBytes = this.Reader.ReadBytes(this.RequestSocket.Available);
                var stream = new MemoryStream(readBytes);
                Package package = (Package)Formatter.Deserialize(stream);

                var data = (LoginInfo)package.Data;
                Console.WriteLine(package.PackageType);
                Console.WriteLine(data.Username);
                Console.WriteLine(data.Password);
            }
        }
    }
}
