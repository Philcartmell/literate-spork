using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Extensions;

namespace Notifications.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void CanRemoveAllCharsExceptLettersOrDigitsFromString()
        {
            Assert.AreEqual("ab23c", "a*b' 23c".RemoveAllExceptLettersOrDigits());
        }

        [TestMethod]
        public void CanRemoveAllCharsExceptNumbersOrPointFromString()
        {
            Assert.AreEqual("1234.56", "£1,234.56".RemoveAllExceptDigitsOrPoint());
        }
    }
}
