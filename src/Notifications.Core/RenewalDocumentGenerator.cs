using Notifications.Core.Models;
using Notifications.Core.Readers;
using Notifications.Core.Writers;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Core
{
    /// <summary>
    /// Converts renewal list into documents
    /// </summary>
    public class RenewalDocumentGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IInputReader _inputReader;
        private readonly IOutputWriter _outputWriter;
        private readonly IRenewalOutputGenerator _renewalOutputGenerator;

        public RenewalDocumentGenerator(IFileSystem fileSystem, IInputReader inputReader,
            IOutputWriter outputWriter, IRenewalOutputGenerator renewalOutputGenerator)
        {
            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));

            if (inputReader == null)
                throw new ArgumentNullException(nameof(inputReader), "Input reader cannot be null");

            if (outputWriter == null)
                throw new ArgumentNullException(nameof(outputWriter), "Output writer cannot be null");

            if (renewalOutputGenerator==null)
                throw new ArgumentNullException(nameof(renewalOutputGenerator), "Renewal output generator cannot be null");

            _fileSystem = fileSystem;
            _inputReader = inputReader;
            _outputWriter = outputWriter;
            _renewalOutputGenerator = renewalOutputGenerator;
        }

        /// <summary>
        /// Creates a file system reader/writer
        /// </summary>
        /// <param name="fileSystem">File system</param>
        /// <param name="sourceFile">Source CSV</param>
        /// <param name="outputPath">Output directory</param>
        /// <param name="templateFile">Template file</param>
        /// <returns>Renewal document generator</returns>
        internal static RenewalDocumentGenerator Create(IFileSystem fileSystem, string sourceFile, string outputPath, string templateFile)
        {
            if (String.IsNullOrEmpty(templateFile))
                throw new ArgumentNullException(nameof(templateFile), "Template file path cannot be null or empty");

            if (!fileSystem.File.Exists(templateFile))
                throw new ArgumentOutOfRangeException(nameof(templateFile), "Template file does not exist");

            var templateContent = fileSystem.File.ReadAllText(templateFile);

            if (String.IsNullOrEmpty(templateContent))
                throw new ArgumentOutOfRangeException(nameof(templateContent), "Template file exists, but empty.");

            var inputReader = new FileInputReader(fileSystem, sourceFile);
            var outputWriter = new FileOutputWriter(fileSystem, outputPath, false);
            var templateGenerator = new RenewalOutputGenerator("dd/MM/yyyy", templateContent);

            return new RenewalDocumentGenerator(fileSystem, inputReader, outputWriter, templateGenerator);
        }

        /// <summary>
        /// Creates a renewal generator that works with basic file system
        /// </summary>
        /// <param name="sourceFile">Source CSV</param>
        /// <param name="outputPath">Output directory</param>
        /// <param name="templatePath">Template file</param>
        /// <returns>Renewal document generator</returns>
        public static RenewalDocumentGenerator Create(string sourceFile, string outputPath, string templatePath)
        {
            return Create(new FileSystem(), sourceFile, outputPath, templatePath);
        }

        /// <summary>
        /// Runs the generation process
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            try
            {
                await _inputReader.ReadInputAsync();

                foreach (var renewal in _inputReader.Items)
                {
                    var decorated = new RenewalItemWithMonthlyBreakdown(renewal, 0.05m);
                    var template = await _renewalOutputGenerator.GenerateAsync(DateTime.Now, decorated);
                    await _outputWriter.WriteAsync(renewal, template);
                }

                if (_inputReader.Errors.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var err in _inputReader.Errors)
                    {
                        sb.AppendLine(err);
                    }
                    await _outputWriter.WriteErrorLogAsync(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                await _outputWriter.WriteErrorLogAsync(ex.Message);
                throw;
            }
        }
    }
}
