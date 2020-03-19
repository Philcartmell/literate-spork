using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Models;
using Notifications.Core.Writers;
using Notifications.Tests.Infrastructure;
using System;

namespace Notifications.Tests.Writers
{
    [TestClass]
    public class FileRenewalOutputWriterTests
    {
        #region ctor tests

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullFileSystemDirectoryThrows()
        {
            new FileOutputWriter(null, @"c:\temp");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullOutputDirectoryThrows()
        {
            new FileOutputWriter(new RenewalsFileSystemMock(), null);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void IfOutputDirectoryDoesNotExistThenThrow()
        {
            new FileOutputWriter(new RenewalsFileSystemMock(), @"c:\abc123");
        }

        #endregion

        #region filename generation

        [TestMethod]
        public void FilenameStripsOutWhiteSpaceFromSurname()
        {
            var frow = new FileOutputWriter(new RenewalsFileSystemMock(), @"c:\temp");
            var renewalItem = new RenewalItem(1, "mr", "joe", "blo ggs", "product a", 123.45m, 999.99m);

            var filename = frow.ComputeFilename(renewalItem);
            
            Assert.AreEqual("1-bloggs.txt", filename);
        }

        [TestMethod]
        public void FilenameStripsOutSpecialCharsFromSurname()
        {
            var frow = new FileOutputWriter(new RenewalsFileSystemMock(), @"c:\temp");
            var renewalItem = new RenewalItem(1, "mr", "joe", "blo/ggs", "product a", 123.45m, 999.99m);

            var filename = frow.ComputeFilename(renewalItem);

            Assert.AreEqual("1-bloggs.txt", filename);
        }

        #endregion

    }
}
