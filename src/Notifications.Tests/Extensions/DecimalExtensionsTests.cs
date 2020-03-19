using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Extensions;

namespace Notifications.Tests.Extensions
{
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void CanCalculate3DecimalPlaces()
        {
            decimal argument = 123.456m;
            Assert.AreEqual(3, argument.CountDecimalPlaces());
        }

        [TestMethod]
        public void CanCalculate2DecimalPlaces()
        {
            decimal argument = 123.45m;
            Assert.AreEqual(2, argument.CountDecimalPlaces());
        }

        [TestMethod]
        public void CanCalculate1DecimalPlace()
        {
            decimal argument = 123.4m;
            Assert.AreEqual(1, argument.CountDecimalPlaces());
        }

        [TestMethod]
        public void CanCalculateNoDecimalPlace()
        {
            decimal argument = 123m;
            Assert.AreEqual(0, argument.CountDecimalPlaces());
        }

        [TestMethod]
        public void CanGetFractionsFromDecimalVariant1()
        {
            decimal argument = 123.456m;
            decimal fraction = argument.GetFraction();
            Assert.AreEqual(0.456m, fraction);
        }

        [TestMethod]
        public void CanGetFractionsFromDecimalVariant2()
        {
            decimal argument = 123m;
            decimal fraction = argument.GetFraction();
            Assert.AreEqual(0m, fraction);
        }

        [TestMethod]
        public void CanRoundDecimalToTwoDecimalPlacesVarient()
        {
            Assert.AreEqual(4.37m, 4.375m.RoundDown());
        }

    }
}
