using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCT.Services;

namespace BCT.ServicesTests
{
    [TestClass]
    public class TokenizationServiceTest
    {
        [TestMethod]
        public void Test1_IsLuhnValid()
        {
            string cardNumber = "79927398713";
            Assert.AreEqual(true, TokenizationService.IsLuhnValid(cardNumber));
        }

        [TestMethod]
        public void Test2_IsLuhnValid()
        {
            string cardNumber = "4563960122019991";
            Assert.AreEqual(true, TokenizationService.IsLuhnValid(cardNumber));
        }
    }
}
