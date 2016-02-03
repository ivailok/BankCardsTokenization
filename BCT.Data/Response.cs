using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Data
{
    public enum ResponseType
    {
        Success,
        Error
    }

    [Serializable]
    public class Response
    {
        public string Message { get; set; }

        public ResponseType ResponseType { get; set; }

        public object Data { get; set; }
    }
}
