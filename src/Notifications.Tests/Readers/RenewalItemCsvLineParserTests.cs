using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Readers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notifications.Tests.Readers
{
    [TestClass]
    public class RenewalItemCsvLineParserTests
    {
        #region ctor

        [TestMethod]
        public void InstantiatingParserInitialisesValidLineDictionary()
        {
            var parser = new RenewalCsvLineReader(',');
            Assert.IsNotNull(parser.ValidLines);
        }

        #endregion

        #region ReadLine tests

        [TestMethod]
        public void LineNumberIndexCorrectlyIncreasesPerParse()
        {
            var parser = new RenewalCsvLineReader(',');
            parser.ReadLine("1,mr,joe,bloggs,product,123.45,123.45");
            Assert.AreEqual(1, parser.CurrentLineNo);
            parser.ReadLine("2,mr,joe,bloggs,product,123.45,123.45");
            Assert.AreEqual(2, parser.CurrentLineNo);
        }

        [TestMethod]
        public void LineIsStoredForNonEmptyLines()
        {
            var parser = new RenewalCsvLineReader(',');

            parser.ReadLine("1,Header,Row");
            parser.ReadLine("1,mr,joe,bloggs,product,123.45,123.45");
            parser.ReadLine("123");

            Assert.AreEqual(2, parser.ValidLines.Count);
        }

        [TestMethod]
        public void LineIsNotStoredForEmptyLinesOrHeader()
        {
            var parser = new RenewalCsvLineReader(',');

            parser.ReadLine("1,Header,Row");
            Assert.AreEqual(0, parser.ValidLines.Count);

            parser.ReadLine(String.Empty);
            parser.ReadLine("123");

            Assert.AreEqual(1, parser.ValidLines.Count);
        }

        #endregion

        #region validate line

        [TestMethod]
        public void InvalidIdFails()
        {
            string validLine = "x,mr,joe,bloggs,product,123.45,123.45";
            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.IsTrue(validationResult.Errors.Count == 1);
        }

        [TestMethod]
        public void EmptyTitleFails()
        {
            string validLine = "1,,joe,bloggs,product,123.45,123.45";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.IsTrue(validationResult.Errors.Count == 1);
            Assert.AreEqual(1, validationResult.Errors.First().Key);
        }

        [TestMethod]
        public void UnsupportedTitleFails()
        {
            string validLine = "1,lord,joe,bloggs,product,123.45,123.45";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.IsTrue(validationResult.Errors.Count == 1);
            Assert.AreEqual(1, validationResult.Errors.First().Key);
            Assert.IsTrue(validationResult.Errors.First().Value.Contains("supported values are"));
        }

        [TestMethod]
        public void EmptyFirstNameFails()
        {
            string validLine = "1,mr,,bloggs,product,123.45,123.45";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void EmptyLastNameFails()
        {
            string validLine = "1,mr,joe,,product,123.45,123.45";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void EmptyProductNameFails()
        {
            string validLine = "1,mr,joe,bloggs,,123.45,123.45";

            var parser = new RenewalCsvLineReader();
            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void EmptyPayoutAmountFails()
        {
            string validLine = "1,mr,joe,bloggs,product,,123.45";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void InvalidPayoutAmountFails()
        {
            string validLine = "1,mr,joe,bloggs,product,xx.xx,123.45";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void EmptyAnnualPremiumFails()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void InvalidAnnualPremiumFails()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,xx.xx";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void AnnualPremiumWithCurrencyOrCommaSuceeds()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
        }

        #endregion

        #region sucessful conversion

        [TestMethod]
        public void GoodLineSetsRenewalItemPropertyAndIdCorrect()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
            Assert.IsNotNull(validationResult.RenewalItem);
            Assert.AreEqual(1, validationResult.RenewalItem.Id);
        }

        [TestMethod]
        public void GoodLineSetsRenewalItemPropertyAndTitleCorrect()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var parser = new RenewalCsvLineReader();
            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
            Assert.IsNotNull(validationResult.RenewalItem);
            Assert.AreEqual("mr", validationResult.RenewalItem.Title);
        }

        [TestMethod]
        public void GoodLineSetsRenewalItemPropertyAndFirstNameCorrect()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
            Assert.IsNotNull(validationResult.RenewalItem);
            Assert.AreEqual("joe", validationResult.RenewalItem.FirstName);
        }

        [TestMethod]
        public void GoodLineSetsRenewalItemPropertyAndSurnameCorrect()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var parser = new RenewalCsvLineReader();
            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
            Assert.IsNotNull(validationResult.RenewalItem);
            Assert.AreEqual("bloggs", validationResult.RenewalItem.Surname);
        }

        [TestMethod]
        public void GoodLineSetsRenewalItemPropertyAndProductNameCorrect()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
            Assert.IsNotNull(validationResult.RenewalItem);
            Assert.AreEqual("product", validationResult.RenewalItem.ProductName);
        }

        [TestMethod]
        public void GoodLineSetsRenewalItemPropertyAndPayoutAmountCorrect()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
            Assert.IsNotNull(validationResult.RenewalItem);
            Assert.AreEqual(123.45m, validationResult.RenewalItem.PayoutAmount);
        }

        [TestMethod]
        public void GoodLineSetsRenewalItemPropertyAndAnnualPremiumCorrect()
        {
            string validLine = "1,mr,joe,bloggs,product,123.45,£1234.56";

            var validationResult = RenewalCsvLineReader.TryConvertLine(1, validLine.Split(','));

            Assert.AreEqual(0, validationResult.Errors.Count);
            Assert.IsNotNull(validationResult.RenewalItem);
            Assert.AreEqual(1234.56m, validationResult.RenewalItem.AnnualPremium);
        }

        #endregion

        #region validation helpers

        [TestMethod]
        public void AssertingWhetherAStringIsNullOrWhitespacePopulatesDictionaryWithCorrectRowNumber()
        {
            var dict = new Dictionary<int, string>();
            string[] fields = new[] { "", "hello", "world" };

            var validationResult = RenewalCsvLineReader.TryAssertStringIsNullOrWhitespace(fields, 1, 0, "Id", "must have a value", dict);

            Assert.IsTrue(validationResult);
            Assert.AreEqual(1, dict.Count);
            Assert.AreEqual("Id must have a value", dict[1]);
        }

        [TestMethod]
        public void AssertingWhetherAStringIsNullOrWhitespaceReturnsFalseWhenStringIsValid()
        {
            var dict = new Dictionary<int, string>();
            string[] fields = new[] { "", "hello", "world" };

            var validationResult = RenewalCsvLineReader.TryAssertStringIsNullOrWhitespace(fields, 1, 1, "First name", "must have a value", dict);

            Assert.IsFalse(validationResult);
            Assert.AreEqual(0, dict.Count);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void AssertingWhetherAStringIsNullOrWhitespaceAndFIeldIndexOutOfRangeThrows()
        {
            var dict = new Dictionary<int, string>();
            string[] fields = new[] { "", "hello", "world" };
            var validationResult = RenewalCsvLineReader.TryAssertStringIsNullOrWhitespace(fields, 1, 4, "First name", "must have a value", dict);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void AssertingWhetherAStringIsNullOrWhitespaceAndFieldIndexNegativeThrows()
        {
            var dict = new Dictionary<int, string>();
            string[] fields = new[] { "", "hello", "world" };
            var validationResult = RenewalCsvLineReader.TryAssertStringIsNullOrWhitespace(fields, 1, -1, "First name", "must have a value", dict);
        }

        #endregion


    }
}
