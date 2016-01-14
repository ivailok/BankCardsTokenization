using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.ClientStart
{
    class Program
    {
        static void Main(string[] args)
        {
            Client.Client client = new Client.Client();
            while (true)
            {
                client.Send(Convert.ToInt32(Console.ReadLine()));
            }
        }
    }
}
