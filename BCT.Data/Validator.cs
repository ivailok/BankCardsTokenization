using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCT.Data
{
    public class Validator
    {
        public Validator()
        {

        }

        public bool ValidatePassword(string pass)
        {
            return Regex.IsMatch(pass, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@_#$%^&*!]).{6,20}$");
        }

        public bool ValidateUsername(string username)
        {
            return Regex.IsMatch(username, "^(?=.{4,25}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");
        }

        public bool ValidateToken(string token)
        {
            return Regex.IsMatch(token, @"^[12789][0-9]{15}$");
        } 

        public bool ValidateCardNumber(string cardNumber)
        {
            return Regex.IsMatch(cardNumber, @"^[3456][0-9]{15}$");
        }
    }
}
