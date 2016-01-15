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
                Package package = new Package();
                package.PackageType = PackageType.Login;
                package.Data = new LoginInfo()
                {
                    Username = username,
                    Password = password
                };
                client.Send(package);
            }
        }
    }
}
