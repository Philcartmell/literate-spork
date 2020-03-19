using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Models;
using System;
using System.Threading.Tasks;

namespace Notifications.Tests.Models
{
    [TestClass]
    public class RenewalOutputGeneratorTests
    {
        public static DateTime DUMMY_DATE = new DateTime(1901, 12, 13, 14, 15, 16);

        #region ctor

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ProvidingANullDateTimeToStringThrows()
        {
            new RenewalOutputGenerator(null, null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ProvidingAnEmptyDateTimeToStringThrows()
        {
            new RenewalOutputGenerator(String.Empty, String.Empty);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ProvidingANullTemplateThrows()
        {
            new RenewalOutputGenerator("dd/MM/yyyy", null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ProvidingAnEmptyTemplateThrows()
        {
            new RenewalOutputGenerator("dd/MM/yyyy", String.Empty);
        }

        #endregion

        #region Generate method

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AttemptingToGenerateWithNullItemThrows()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "template content");
            renewalOutputGenerator.GenerateAsync(DUMMY_DATE, null);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateLetterDate()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((letter_date))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello " + DUMMY_DATE.ToString("dd/MM/yyyy"), text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateTitle()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((title))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello mr", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateFirstName()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((first_name))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello joe", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateSurname()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((surname))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello bloggs", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateProduct()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((product_name))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello product a", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolatePayoutAmount()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((payout_amount))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello 1000.00", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolatePremiumAmount()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((annual_premium))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello 50.00", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateCreditCharge()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((credit_charge))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello 2.50", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateFirstPayment()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((first_payment))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello 4.43", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateOtherPayments()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((subsequent_payment))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello 4.37", text);
        }

        [TestMethod]
        public async Task CanCorrectlyInterpolateTotalPremiumDirectDebit()
        {
            var renewalOutputGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "hello ((total_premium_dd))");

            var ri = new RenewalItem(1, "mr", "joe", "bloggs", "product a", 1000m, 50m);
            var ric = new RenewalItemWithMonthlyBreakdown(ri, 0.05m);
            var text = await renewalOutputGenerator.GenerateAsync(DUMMY_DATE, ric);
            Assert.AreEqual("hello 52.50", text);
        }

        #endregion

    }
}
