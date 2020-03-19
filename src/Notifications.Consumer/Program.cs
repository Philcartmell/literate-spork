using Notifications.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sourceFile = String.Empty;
            string outputPath = String.Empty;
            string template = String.Empty;

            if (Debugger.IsAttached && args.Length == 0)
            {
                var baseDirectory = Path.Combine(Environment.CurrentDirectory, @"..\..\Data");
                sourceFile = Path.Combine(baseDirectory, "Customer.csv");
                outputPath = Path.Combine(baseDirectory, @".\Output");
                template = Path.Combine(baseDirectory, "renewal_template.txt");
            }
            else if (args.Length == 3)
            {
                sourceFile = args[0];
                outputPath = args[1];
                template = args[2];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No arguments provided, example usage:");
                Console.WriteLine(@"notifications-consumer.exe source.csv c:\temp\outputDirectory c:\temp\template.txt");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            try
            {
                var generator = RenewalDocumentGenerator.Create(sourceFile, outputPath, template);
                await generator.RunAsync();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("");
                Console.WriteLine("Program has finished running.");
            }
        }
    }
}
