using Notifications.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Notifications.Core.Readers
{
    /// <summary>
    /// Reads input data from file
    /// </summary>
    public class FileInputReader : IInputReader
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _sourceFile;
        internal readonly RenewalCsvLineReader RenewalItemCsvReader;

        /// <summary>
        /// Valid items
        /// </summary>
        public IList<IRenewalItem> Items { get; }

        /// <summary>
        /// File column delimiter
        /// </summary>
        public char Delimiter { get; private set; }

        /// <summary>
        /// List of errors whilst parsing file
        /// </summary>
        public IList<string> Errors { get; }

        /// <summary>
        /// FileRenewalInputReader with comma delimiter
        /// </summary>
        /// <param name="fileSystem">IFileSystem</param>
        /// <param name="sourceFile">Source file path</param>
        public FileInputReader(IFileSystem fileSystem, string sourceFile)
            : this(fileSystem, sourceFile, ',')
        {

        }

        private FileInputReader()
        {
            Items = new List<IRenewalItem>();
            Errors = new List<string>();
        }

        /// <summary>
        /// FileRenewalInputReader
        /// </summary>
        /// <param name="fileSystem">IFileSystem</param>
        /// <param name="sourceFile">Source file path</param>
        /// <param name="delimiter">Source file delimiter</param>
        public FileInputReader(IFileSystem fileSystem, string sourceFile, char delimiter) : this()
        {
            Delimiter = delimiter;
            RenewalItemCsvReader = new RenewalCsvLineReader(delimiter);

            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));

            if (String.IsNullOrEmpty(sourceFile))
                throw new ArgumentNullException(nameof(sourceFile));

            if (!fileSystem.File.Exists(sourceFile))
                throw new ArgumentOutOfRangeException(nameof(sourceFile), "Source file does not exist");

            _fileSystem = fileSystem;
            _sourceFile = sourceFile;
        }

        /// <summary>
        /// Processes the renewal items
        /// </summary>
        /// <returns></returns>
        public async Task ReadInputAsync()
        {
            using (StreamReader streamReader = new StreamReader(_fileSystem.File.OpenRead(_sourceFile)))
            {
                string line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    RenewalItemCsvReader.ReadLine(line);
                }
            }

            foreach (var validLine in RenewalItemCsvReader.ValidLines)
            {
                var conversion = RenewalCsvLineReader.TryConvertLine(validLine.Key, validLine.Value);

                if (conversion.RenewalItem != null)
                {
                    Items.Add(conversion.RenewalItem);
                }
                else
                {
                    Errors.Add($"Line {validLine.Key+1}: " + String.Join(",", conversion.Errors[validLine.Key]));
                }
            }

        }
    }
}
