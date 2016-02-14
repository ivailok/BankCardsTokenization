using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Data
{
    public enum Rights
    {
        CanRegisterToken = 1,
        CanGetCardNumber = 2
    }

    [Serializable]
    public class Register
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int Rights { get; set; }
    }
}
