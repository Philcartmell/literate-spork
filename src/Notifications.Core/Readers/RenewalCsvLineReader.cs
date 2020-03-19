using Notifications.Core.Extensions;
using Notifications.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notifications.Core.Readers
{
    /// <summary>
    /// Parses renewal CSV lines
    /// </summary>
    public class RenewalCsvLineReader
    {
        private readonly char _delimiter;

        /// <summary>
        /// Stores the current line no. of lines parsed
        /// </summary>
        public int CurrentLineNo { get; private set; }

        /// <summary>
        /// Stores valid lines split by delimiter
        /// </summary>
        public Dictionary<int, string[]> ValidLines { get; private set; }

        /// <summary>
        /// Instantiate a RenewalItemLineParser with specified field delimiter
        /// </summary>
        /// <param name="delimiter">Field delimiter</param>
        public RenewalCsvLineReader(char delimiter)
        {
            ValidLines = new Dictionary<int, string[]>();
            _delimiter = delimiter;
        }

        /// <summary>
        /// Instantiate a RenewalItemLineParser with default (comma) field delimiter
        /// </summary>
        public RenewalCsvLineReader() : this(',')
        {
            
        }

        /// <summary>
        /// Read a CSV line
        /// </summary>
        /// <param name="line">The line</param>
        public void ReadLine(string line)
        {
            if (CurrentLineNo>0 && !String.IsNullOrWhiteSpace(line))
            {
                string[] split = line.Split(_delimiter);
                ValidLines.Add(CurrentLineNo, split);
            }
            CurrentLineNo++;
        }

        /// <summary>
        /// Tries to convert a CSV line
        /// </summary>
        /// <param name="fields">Fields</param>
        /// <returns>Conversion and possible validation errors</returns>
        internal static RenewalItemConversionResult TryConvertLine(int lineNumber, string[] fields)
        {
            /*
             * expected fields spec
             *   0     1     2      3        4         5        6
             * |---+------+-----+--------+---------+--------+---------|
               | 1 | lord | joe | bloggs | product | 123.45 | 123.45  |
               |---+------+-----+--------+---------+--------+---------|
             */

            var result = new RenewalItemConversionResult();

            int parsedId;
            if (!Int32.TryParse(fields[0], out parsedId))
            {
                result.Errors.AddOrConcatentate(lineNumber, $"Cannot convert {fields[0]} to number"); 
            }

            #region title

            string title = fields[1];

            TryAssertStringIsNullOrWhitespace(fields, lineNumber, 1, "Title", "must have a value", result.Errors);

            string[] supportedTitles = new[] { "miss", "mr", "mrs" };

            if (!supportedTitles.Contains(title.ToLowerInvariant()))
            {
                result.Errors.AddOrConcatentate(lineNumber, "Invalid title, supported values are: " + String.Join(", ", supportedTitles));
            }

            #endregion

            string firstName = fields[2];
            TryAssertStringIsNullOrWhitespace(fields, lineNumber, 2, "FirstName", "must have a value", result.Errors);

            string surname = fields[3];
            TryAssertStringIsNullOrWhitespace(fields, lineNumber, 3, "Surname", "must have a value", result.Errors);
            
            string productName = fields[4];
            TryAssertStringIsNullOrWhitespace(fields, lineNumber, 4, "ProductName", "must have a value", result.Errors);

            #region payout amount

            TryAssertStringIsNullOrWhitespace(fields, lineNumber, 5, "PayoutAmount", "must have a value", result.Errors);

            decimal payoutAmountParsed;
            if (!Decimal.TryParse(fields[5].RemoveAllExceptDigitsOrPoint(), out payoutAmountParsed))
            {
                result.Errors.AddOrConcatentate(lineNumber, $"Cannot convert {fields[5]} to decimal");
            }

            #endregion

            #region annual premium

            TryAssertStringIsNullOrWhitespace(fields, lineNumber, 6, "AnnualPremium", "must have a value", result.Errors);

            decimal annualPremiumParsed;
            if (!Decimal.TryParse(fields[6].RemoveAllExceptDigitsOrPoint(), out annualPremiumParsed))
            {
                result.Errors.AddOrConcatentate(lineNumber, $"Cannot convert {fields[6]} to decimal");
            }

            #endregion

            if (!result.Errors.Any())
            {
                result.RenewalItem = new RenewalItem(parsedId, title, firstName, surname, productName, payoutAmountParsed, annualPremiumParsed); 
            }

            return result;
        }

        
        internal static bool TryAssertStringIsNullOrWhitespace(string[] fields, int lineNumber, int fieldIndex, string fieldName, string validationMessage, Dictionary<int, string> dict)
        {
            if (fieldIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(fieldIndex));

            if (fieldIndex > fields.Length-1)
                throw new ArgumentOutOfRangeException(nameof(fieldIndex));

            if (!String.IsNullOrWhiteSpace(fields[fieldIndex]))
                return false;

            dict.AddOrConcatentate(lineNumber, fieldName + " " + validationMessage);
            return true;
        }

    }
}
