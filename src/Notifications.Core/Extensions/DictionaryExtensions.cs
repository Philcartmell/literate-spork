using System.Collections.Generic;

namespace Notifications.Core.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Concatentate a string to dictionary value if key exists
        /// </summary>
        /// <param name="dict">Dictionary</param>
        /// <param name="index">Key</param>
        /// <param name="value">Value to concatenate</param>
        /// <param name="delimiter">Delimiter to use</param>
        public static void AddOrConcatentate(this Dictionary<int, string> dict, int index, string value, string delimiter)
        {
            if (dict.ContainsKey(index))
            {
                dict[index] = dict[index] + delimiter + value;
            }
            else
            {
                dict.Add(index, value);
            }
        }

        /// <summary>
        /// Concatentate a string to dictionary value if key exists using comma space delimiter
        /// </summary>
        /// <param name="dict">Dictionary</param>
        /// <param name="index">Key</param>
        /// <param name="value">Value to concatenate</param>
        public static void AddOrConcatentate(this Dictionary<int, string> dict, int index, string value)
        {
            AddOrConcatentate(dict, index, value, ". ");
        }

    }
}
