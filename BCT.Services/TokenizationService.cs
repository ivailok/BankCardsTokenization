using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Services
{
    public class TokenizationService
    {
        public static bool IsLuhnValid(string cardNumber)
        {
            int checksum = 0;
            int doubledDigit = 0;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                if (((cardNumber.Length - i) & 1) == 1)
                {
                    checksum += cardNumber[i] - '0';
                }
                else
                {
                    doubledDigit = 2 * (cardNumber[i] - '0');
                    if (doubledDigit >= 10)
                    {
                        checksum += 1 + doubledDigit % 10;
                    }
                    else
                    {
                        checksum += doubledDigit;
                    }
                }
            }

            return checksum % 10 == 0;
        }
    }
}
