using System;
using System.Threading.Tasks;

namespace Notifications.Core.Models
{
    /// <summary>
    /// Interface for generating some interpolated output from a for a RenewalItem
    /// </summary>
    public interface IRenewalOutputGenerator
    {
        /// <summary>
        /// Returns the template interpolated with renewal item data
        /// </summary>
        /// <param name="letterTimestamp">Date to use for letter date token</param>
        /// <param name="renewalItem">Renewal item</param>
        /// <returns>Finished template</returns>
        Task<string> GenerateAsync(DateTime letterTimestamp, IRenewalItem renewalItem);
    }
}