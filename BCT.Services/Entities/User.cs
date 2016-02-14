using BCT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Services.Entities
{ 
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int Rights { get; set; }
    }
}
