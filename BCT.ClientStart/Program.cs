using BCT.Data;
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
                Console.Write("username: ");
                string username = Console.ReadLine();
                Console.Write("password: ");
                string password = Console.ReadLine();
                var login = new Login()
                {
                    Username = username,
                    Password = password
                };
                client.Send(login, RequestType.Login);
            }
        }
    }
}
