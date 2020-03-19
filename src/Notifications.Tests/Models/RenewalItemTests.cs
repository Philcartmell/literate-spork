using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Models;
using System;

namespace Notifications.Tests
{
    [TestClass]
    public class RenewalItemTests
    {
        #region id

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void InvalidIdThrows()
        {
            new RenewalItem(-1, "Mr", "Joe", "Bloggs", "ABC", 123m, 456m);
        }

        #endregion

        #region title
        
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullTitleThrows()
        {
            new RenewalItem(0, null, "Joe", "Bloggs", "ABC", 123m, 456m);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void EmptyTitleThrows()
        {
            new RenewalItem(0, String.Empty, "Joe", "Bloggs", "ABC", 123m, 456m);
        }

        #endregion

        #region first name

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullFirstNameThrows()
        {
            new RenewalItem(0, "Mr", null, "Bloggs", "ABC", 123m, 456m);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void EmptyFirstNameThrows()
        {
            new RenewalItem(0, "Mr", String.Empty, "Bloggs", "ABC", 123m, 456m);
        }

        #endregion

        #region last name

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullLastNameThrows()
        {
            new RenewalItem(0, "Mr", "Joe", String.Empty, "ABC", 123m, 456m);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void EmptyLastNameThrows()
        {
            new RenewalItem(0, "Mr", "Joe", null, "ABC", 123m, 456m);
        }

        #endregion

        #region product name

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullProductNameThrows()
        {
            new RenewalItem(0, "Mr", "Joe", "Bloggs", null, 123m, 456m);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void EmptyProductNameThrows()
        {
            new RenewalItem(0, "Mr", "Joe", "Bloggs", String.Empty, 123m, 456m);
        }

        #endregion

        #region payout amount

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void ZeroPayoutAmountThrows()
        {
            new RenewalItem(0, "Mr", "Joe", "Bloggs", "ABC", 0m, 456m);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void NegativePayoutAmountThrows()
        {
            new RenewalItem(0, "Mr", "Joe", "Bloggs", "ABC", -10m, 456m);
        }

        #endregion

        #region annual premium

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void ZeroAnnualPremiumAmountThrows()
        {
            new RenewalItem(0, "Mr", "Joe", "Bloggs", "ABC", 10m, 0m);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void NegativeAnnualPremiumAmountThrows()
        {
            new RenewalItem(0, "Mr", "Joe", "Bloggs", "ABC", 10m, -10m);
        }

        #endregion

    }
}
