using BCT.Data;
using BCT.Services.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Services
{
    public class UsersService
    {
        private XmlFileService storage;
        private Dictionary<string, string> users;

        public UsersService()
        {
            this.storage = new XmlFileService("users.xml");
            this.users = new Dictionary<string, string>();

            if (File.Exists("users.xml"))
            {
                var collection = this.storage.Load(typeof(User[])) as User[];
                foreach (var item in collection)
                {
                    this.users.Add(item.Username, item.Password);
                }
            }
        }

        public void Login(Login login)
        {
            if (!this.users.ContainsKey(login.Username) || 
                this.users[login.Username] != login.Password)
            {
                throw new Exception("Username or password are wrong.");
            }
        }
    }
}
