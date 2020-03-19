using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace Notifications.Tests.Infrastructure
{
    internal class RenewalsFileSystemMock : MockFileSystem
    {
        public const string MOCK_DATA_CORRECT = "ID,Title,FirstName,Surname,ProductName,PayoutAmount,AnnualPremium\n" +
"1,Miss,Sally,Smith,Standard Cover,190820.00,123.45\n" +
"2,Mr,John,Smith,Enhanced Cover,83205.50,120.00\n" +
"3,Mrs,Helen,Daniels,Special Cover,200000.99,141.20\n" +
"x,Mrs,Helen,Daniels,Special Cover,200000.99,141.20\n" +
"y,Mrs,Helen,Daniels,Special Cover,,141.20";

        public const string TEMPLATE_CONTENTS = "template data";

        public RenewalsFileSystemMock() : base(new Dictionary<string, MockFileData>
            {
                { @"c:\temp", new MockDirectoryData() },
                { @"c:\temp\renewals.txt", new MockFileData(MOCK_DATA_CORRECT) }
            })
        {
            
        }
    }
}
