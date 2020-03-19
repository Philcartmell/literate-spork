using Notifications.Core.Extensions;
using Notifications.Core.Models;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Notifications.Core.Writers
{
    /// <summary>
    /// Writes output to file system
    /// </summary>
    public class FileOutputWriter : IOutputWriter
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _outputDirectory;
        private readonly bool _overwriteIfExists;

        #region ctors

        /// <summary>
        /// Instantiate a FileRenewalOutputWriter saving into specified location, will not overwrite pre-existing files.
        /// </summary>
        /// <param name="outputDirectory">Directory path (must exist)</param>
        public FileOutputWriter(string outputDirectory) : this(new FileSystem(), outputDirectory)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputDirectory">Directory path (must exist)</param>
        /// <param name="overwriteIfExists">Overwrite file if exists</param>
        public FileOutputWriter(string outputDirectory, bool overwriteIfExists)
            : this(new FileSystem(), outputDirectory, overwriteIfExists)
        {
        }

        /// <summary>
        /// Instantiate a FileRenewalOutputWriter saving into specified location, will not overwrite pre-existing files.
        /// </summary>
        /// <param name="fileSystem">File system to use</param>
        /// <param name="outputDirectory">Directory path (must exist)</param>
        public FileOutputWriter(IFileSystem fileSystem, string outputDirectory)
            : this(fileSystem, outputDirectory, false)
        {
            
        }

        /// <summary>
        /// Instantiate a FileRenewalOutputWriter saving into specified location, will not overwrite pre-existing files.
        /// </summary>
        /// <param name="fileSystem">File system to use</param>
        /// <param name="outputDirectory">Directory path (must exist)</param>
        /// <param name="overwriteIfExists">Overwrite file if exists</param>
        public FileOutputWriter(IFileSystem fileSystem, string outputDirectory, bool overwriteIfExists)
        {
            if (fileSystem==null)
                throw new ArgumentNullException(nameof(fileSystem));

            if (String.IsNullOrEmpty(outputDirectory))
                throw new ArgumentNullException(nameof(outputDirectory));

            if (!fileSystem.Directory.Exists(outputDirectory))
                throw new ArgumentOutOfRangeException(nameof(outputDirectory), "Directory must already exist");

            _fileSystem = fileSystem;
            _outputDirectory = outputDirectory;
            _overwriteIfExists = overwriteIfExists;
        }

        #endregion

        public async Task WriteAsync(IRenewalItem renewalItem, string content)
        {
            var filename = ComputeFilename(renewalItem);
            var filePath = Path.Combine(_outputDirectory, filename);
            if (_overwriteIfExists || !File.Exists(filePath))
            {
                _fileSystem.File.WriteAllText(filePath, content);
            }
        }

        public async Task WriteErrorLogAsync(string content)
        {
            var filePath = Path.Combine(_outputDirectory, "_ERRORS.txt");
            if (_overwriteIfExists || !File.Exists(filePath))
            {
                _fileSystem.File.WriteAllText(filePath, content);
            }
        }

        internal string ComputeFilename(IRenewalItem renewalItem)
        {
            return $"{renewalItem.Id.ToString()}-{renewalItem.Surname.RemoveAllExceptLettersOrDigits()}.txt";
        }
    }
}
