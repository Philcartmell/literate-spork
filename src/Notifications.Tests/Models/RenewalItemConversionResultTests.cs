using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Models;

namespace Notifications.Tests.Models
{
    [TestClass]
    public class RenewalItemConversionResultTests
    {
        [TestMethod]
        public void InstantiatingClassInitialisesDictionary()
        {
            var result = new RenewalItemConversionResult();
            Assert.IsNotNull(result.Errors);
        }
    }
}
