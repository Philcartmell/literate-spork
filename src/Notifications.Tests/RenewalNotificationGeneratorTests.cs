using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notifications.Core;
using Notifications.Core.Models;
using Notifications.Core.Readers;
using Notifications.Core.Writers;
using Notifications.Tests.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Tests.Application
{
    [TestClass]
    public class RenewalNotificationGeneratorTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void IfFileSystemIsNullThrow()
        {
            var fileSystem = new RenewalsFileSystemMock();

            var inputReader = new FileInputReader(fileSystem, @"c:\temp\renewals.txt");
            var outputWriter = new FileOutputWriter(fileSystem, @"c:\temp", false);
            var templateGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "content");

            new RenewalDocumentGenerator(null, inputReader, outputWriter, templateGenerator);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void IfOutputWriterIsNullThrow()
        {
            var fileSystem = new RenewalsFileSystemMock();

            var inputReader = new FileInputReader(fileSystem, @"c:\temp\renewals.txt");
            var templateGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "content");

            new RenewalDocumentGenerator(fileSystem, inputReader, null, templateGenerator);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void IfIRenewalOutputGeneratorIsNullThrow()
        {
            var fileSystem = new RenewalsFileSystemMock();

            var inputReader = new FileInputReader(fileSystem, @"c:\temp\renewals.txt");
            var outputWriter = new FileOutputWriter(fileSystem, @"c:\temp", false);

            new RenewalDocumentGenerator(fileSystem, inputReader, outputWriter, null);
        }
        

        [TestMethod]
        public async Task WritesAFileForEachItemInRenewalsFile()
        {
            var fileSystem = new RenewalsFileSystemMock();

            var inputReader = new FileInputReader(fileSystem, @"c:\temp\renewals.txt");
            var outputWriter = new FileOutputWriter(fileSystem, @"c:\temp", false);
            var templateGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "content");

            var renewalNotificationGenerator = new RenewalDocumentGenerator(fileSystem, inputReader, outputWriter, templateGenerator);
            await renewalNotificationGenerator.RunAsync();

            Assert.AreEqual(5, fileSystem.AllFiles.Count());
        }

        [TestMethod]
        public async Task WritesErrorFileForInvalidLines()
        {
            var fileSystem = new RenewalsFileSystemMock();

            var inputReader = new FileInputReader(fileSystem, @"c:\temp\renewals.txt");
            var outputWriter = new FileOutputWriter(fileSystem, @"c:\temp", false);
            var templateGenerator = new RenewalOutputGenerator("dd/MM/yyyy", "content");

            var renewalNotificationGenerator = new RenewalDocumentGenerator(fileSystem, inputReader, outputWriter, templateGenerator);
            await renewalNotificationGenerator.RunAsync();

            Assert.AreEqual(5, fileSystem.AllFiles.Count());
        }


        #region factory convenience method

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task IfTemplateFileNullThrow()
        {
            var fileSystem = new RenewalsFileSystemMock();
            RenewalDocumentGenerator.Create(fileSystem, @"c:\temp\renewals.txt", @"c:\temp", null);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public async Task IfTemplateFileDoesntExistThrow()
        {
            var fileSystem = new RenewalsFileSystemMock();
            RenewalDocumentGenerator.Create(fileSystem, @"c:\temp\renewals.txt", @"c:\temp", @"c:\temp\template.txt");
        }

        #endregion

    }
}
