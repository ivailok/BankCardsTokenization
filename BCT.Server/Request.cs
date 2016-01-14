using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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

        public Request(Socket requestSocket)
        {
            this.RequestSocket = requestSocket;
            this.SocketStream = new NetworkStream(this.RequestSocket);
            this.Reader = new BinaryReader(this.SocketStream);
            this.Writer = new BinaryWriter(this.SocketStream);
        }

        public void Execute()
        {
            while (true)
            {
                while (this.RequestSocket.Available == 0)
                {
                    Thread.Sleep(1000);
                }

                int digit = this.Reader.ReadInt32();
                Console.WriteLine(digit);
            }
        }
    }
}
