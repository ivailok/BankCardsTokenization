using BCT.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCT.Server
{
    public class Server
    {
        private const string LocalHost = "127.0.0.1";
        private const int Port = 44500;
        private const int ActiveThreadsCount = 10;

        private readonly TcpListener Listener;
        private readonly Thread ConnectionThread;

        public Server()
        {
            IPAddress address = IPAddress.Parse(LocalHost);
            this.Listener = new TcpListener(address, Port);
            ThreadPool.SetMinThreads(4, 4);
            ThreadPool.SetMaxThreads(100, 100);
            this.ConnectionThread = new Thread(this.Listen);
            this.ConnectionThread.Start();
        }
        
        public void Listen()
        {
            this.Listener.Start();
            while (true)
            {
                Socket socket = this.Listener.AcceptSocket();
                var request = new RequestHandler(socket);
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate (object state)
                    {
                        request.Execute();
                    }), null);
            }
        }
    }
}
