using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Models;
using System;

namespace Notifications.Tests.Models
{
    [TestClass]
    public class RenewalItemWithMonthlyBreakdownTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void IfRenewalItemIsNullThrows()
        {
            new RenewalItemWithMonthlyBreakdown(null, 0m);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void IfCreditChargeInterestRateIsLessThanZeroThrow()
        {
            new RenewalItemWithMonthlyBreakdown(null, -1m);
        }

        [TestMethod]
        public void CanCalculateCreditChargeCorrectly_Varient1_FivePercent()
        {
            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            Assert.AreEqual(2.5m, ric.CreditCharge);
        }

        [TestMethod]
        public void CanCalculateCreditChargeCorrectly_Varient2_TwentyPercent()
        {
            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.2m);
            Assert.AreEqual(10m, ric.CreditCharge);
        }

        [TestMethod]
        public void TotalPremiumDirectDebitIsAnnualChargePlusCreditCharge()
        {
            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            Assert.AreEqual(52.5m, ric.TotalPremiumDirectDebit);
        }

        [TestMethod]
        public void AverageMonthlyPremiumCalculatesCorrectly()
        {
            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            Assert.AreEqual(4.375m, ric.AverageMonthlyPremium);
        }

        [TestMethod]
        public void MonthlyPaymentDivideDividesConsistently()
        {
            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 100m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            Assert.AreEqual(8.75m, ric.FirstMonthlyPaymentAmount);
            Assert.AreEqual(8.75m, ric.SubsequentMonthlyPaymentAmount);
        }

        [TestMethod]
        public void FirstPaymentMoreThanSubsequent()
        {
            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            Assert.AreEqual(4.43m, ric.FirstMonthlyPaymentAmount);
            Assert.AreEqual(4.37m, ric.SubsequentMonthlyPaymentAmount);
        }
    }
}
