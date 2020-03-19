using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Extensions;
using System.Collections.Generic;

namespace Notifications.Tests.Extensions
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        [TestMethod]
        public void AddingNewItemToDictionaryCreatesNewEntry()
        {
            var dict = new Dictionary<int, string>();
            dict.AddOrConcatentate(0, "hello");
            Assert.AreEqual("hello", dict[0]);
        }

        [TestMethod]
        public void AddingNewExistingItemConcatenatesWithSpecifiedDelimiter()
        {
            var dict = new Dictionary<int, string>();
            dict.AddOrConcatentate(0, "hello");
            dict.AddOrConcatentate(0, "world", "|");
            Assert.AreEqual("hello|world", dict[0]);
        }

        [TestMethod]
        public void AddingNewExistingItemConcatenatesWithDefaultDelimiter()
        {
            var dict = new Dictionary<int, string>();
            dict.AddOrConcatentate(0, "hello");
            dict.AddOrConcatentate(0, "world");
            Assert.AreEqual("hello. world", dict[0]);
        }
    }
}
