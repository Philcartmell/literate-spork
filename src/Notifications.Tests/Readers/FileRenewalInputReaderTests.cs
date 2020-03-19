using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core.Readers;
using Notifications.Tests.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Notifications.Tests.Readers
{
    [TestClass]
    public class FileRenewalInputReaderTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void IfFileSystemIsNullThrow()
        {
            new FileInputReader(null, @"c:\temp\renewals.txt");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void IfSourceFileIsNull()
        {
            new FileInputReader(new RenewalsFileSystemMock(), null);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void IfSourceFileDoesntExistThrow()
        {
            new FileInputReader(new RenewalsFileSystemMock(), @"c:\temp\ren.txt");
        }

        [TestMethod]
        public void DelimiterStoredAsProp()
        {
            var frir = new FileInputReader(new RenewalsFileSystemMock(), @"c:\temp\renewals.txt", ',');
            Assert.AreEqual(',', frir.Delimiter);
        }

        [TestMethod]
        public void CtorInitializesList()
        {
            var frir = new FileInputReader(new RenewalsFileSystemMock(), @"c:\temp\renewals.txt", ',');
            Assert.IsNotNull(frir.Items);
        }

        [TestMethod]
        public void CtorInitializesFailuresDictionary()
        {
            var frir = new FileInputReader(new RenewalsFileSystemMock(), @"c:\temp\renewals.txt", ',');
            Assert.IsNotNull(frir.Errors);
        }

        [TestMethod]
        public async Task CorrectLinesReadFromFileAreCorrectlyIdentified()
        {
            var reader = new FileInputReader(new RenewalsFileSystemMock(), @"c:\temp\renewals.txt", ',');
            await reader.ReadInputAsync();
            Assert.AreEqual(3, reader.Items.Count);
        }

    }
}
