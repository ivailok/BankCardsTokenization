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
        private readonly Thread[] WorkerThreads;
        private readonly Request[] Requests;

        public Server()
        {
            IPAddress address = IPAddress.Parse(LocalHost);
            this.Listener = new TcpListener(address, Port);
            this.WorkerThreads = new Thread[ActiveThreadsCount];
            this.Requests = new Request[ActiveThreadsCount];
            this.ConnectionThread = new Thread(this.Listen);
            this.ConnectionThread.Start();
        }

        private int GetFreeThreadIndex()
        {
            for (int i = 0; i < ActiveThreadsCount; i++)
            {
                if (this.WorkerThreads[i] == null || !this.WorkerThreads[i].IsAlive)
                {
                    return i;
                }
            }
            throw new InvalidOperationException("No free threads.");
        }

        public void Listen()
        {
            this.Listener.Start();
            while (true)
            {
                Socket socket = this.Listener.AcceptSocket();
                try
                {
                    var index = this.GetFreeThreadIndex();
                    this.Requests[index] = new Request(socket);
                    this.WorkerThreads[index] = new Thread(new ThreadStart(this.Requests[index].Execute));
                    this.WorkerThreads[index].Start();
                }
                catch (InvalidOperationException e)
                {
                    // return error
                }
            }
        }
    }
}
