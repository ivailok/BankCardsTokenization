using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Data
{
    public enum RequestType
    {
        Login,
        Logout,
        RegisterToken,
        GetCardNumber,
        Terminate
    }

    [Serializable]
    public class Request
    {
        public object Data { get; set; }

        public RequestType RequestType { get; set; }
    }
}
