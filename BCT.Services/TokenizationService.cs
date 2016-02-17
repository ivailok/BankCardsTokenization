using BCT.Services.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Services
{
    public class TokenizationService
    {
        private const string CardsFilename = "cards.xml";
        private const int CardLength = 16;
        private const string Digits = "0123456789";

        private static XmlFileService storage;
        private static Dictionary<string, Card> cards;
        private static Random randomGenerator;

        static TokenizationService()
        {
            storage = new XmlFileService(CardsFilename, typeof(Card[]));
            cards = new Dictionary<string, Card>();
            randomGenerator = new Random();

            if (File.Exists(CardsFilename))
            {
                var collection = storage.Load() as Card[];
                foreach (var item in collection)
                {
                    cards.Add(item.Token, item);
                }
            }
        }

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

        private bool ContainsOnlyNumbers(string cardNumber)
        {
            return cardNumber.All(x => x >= '0' && x <= '9');
        }

        public bool RegisterToken(string cardNumber, out string token)
        {
            token = null;

            if (!string.IsNullOrEmpty(cardNumber) &&
                ContainsOnlyNumbers(cardNumber) &&
                cardNumber.Length == CardLength && 
                cardNumber[0] >= '3' && cardNumber[0] <= '6' && 
                IsLuhnValid(cardNumber))
            {
                lock (cards)
                {
                    string possibleToken;
                    do
                    {
                        possibleToken = GeneratePossibleToken(cardNumber);
                    } while (cards.ContainsKey(possibleToken));

                    cards.Add(possibleToken, new Card() { CardNumber = cardNumber, Token = possibleToken });
                    storage.Save(cards.Values.ToArray());
                    token = possibleToken;
                }

                return true;
            }

            return false;
        }

        public string GetCardNumber(string token)
        {
            if (string.IsNullOrEmpty(token) ||
                !ContainsOnlyNumbers(token) ||
                token.Length != CardLength ||
                token[0] == '0' ||
                (token[0] >= '3' && token[0] <= '6') ||
                token.Sum(c => c - '0') % 10 == 0)
            {
                throw new FormatException("Invalid token.");
            }

            lock (cards)
            {
                if (cards.ContainsKey(token))
                {
                    return cards[token].CardNumber;
                }
                else
                {
                    throw new KeyNotFoundException("No card matches this token.");
                }
            }
        }
        
        private static string GeneratePossibleToken(string cardNumber)
        {
            char[] generatedToken = new char[CardLength];
            int digitsSum = 0;

            int first;
            do
            {
                first = randomGenerator.Next(1, 10);
            } while ((first >= 3 && first <= 6) || first == 0);
            generatedToken[0] = Digits[first];
            digitsSum += first;

            for (int i = 1; i <= 4; i++)
            {
                generatedToken[CardLength - i] = cardNumber[CardLength - i];
                digitsSum += generatedToken[CardLength - i] - '0';
            }

            for (int i = 1; i < CardLength - 5; i++)
            {
                int d;
                do
                {
                    d = randomGenerator.Next(0, 10);
                } while (Digits[d] == cardNumber[i]);
                generatedToken[i] = Digits[d];
                digitsSum += d;
            }
            
            do
            {
                generatedToken[CardLength - 5] = Digits[randomGenerator.Next(0, 10)];
            } while (generatedToken[CardLength - 5] == cardNumber[CardLength - 5] || 
                    (digitsSum + generatedToken[CardLength - 5] - '0') % 10 == 0);
            //generatedToken[i] = Digits[d];

            return new string(generatedToken);
        }

        public List<Card> GetEntries()
        {
            List<Card> cardsList = cards.Values.ToList();
            return cardsList;
        }
    }
}
