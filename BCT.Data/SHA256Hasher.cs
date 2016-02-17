using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Data
{
    public class SHA256Hasher
    {
        private readonly SHA256 sha;

        public SHA256Hasher()
        {
            this.sha = SHA256.Create();
        }
        
        public string ComputeHash(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var hashBytes = this.sha.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(string.Format("{0:x2}", b));
            }
            return sb.ToString();
        }
    }
}
