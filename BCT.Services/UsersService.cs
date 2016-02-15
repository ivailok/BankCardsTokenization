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
        private const string UsersFilename = "users.xml";

        private static XmlFileService storage;
        private static Dictionary<string, User> users;

        static UsersService()
        {
            storage = new XmlFileService(UsersFilename, typeof(User[]));
            users = new Dictionary<string, User>();

            if (File.Exists(UsersFilename))
            {
                var collection = storage.Load() as User[];
                foreach (var item in collection)
                {
                    users.Add(item.Username, item);
                }
            }
        }

        public void Login(Login login)
        {
            lock (users)
            {
                if (!users.ContainsKey(login.Username) ||
                users[login.Username].Password != login.Password)
                {
                    throw new Exception("Username or password are wrong.");
                }
            }
        }

        public void Register(Register register)
        {
            {
                if (users.ContainsKey(register.Username))
                {
                    throw new Exception("Username taken.");
                }
                else
                {
                    users.Add(register.Username, new User()
                    {
                        Username = register.Username,
                        Password = register.Password,
                        Rights = register.Rights
                    });
                    storage.Save(users.Values.ToArray());
                }
            }
        }

        public int GetRights(string username)
        {
            return users[username].Rights;
        }
    }
}
