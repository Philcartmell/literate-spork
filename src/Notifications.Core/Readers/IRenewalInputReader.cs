using Notifications.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Core.Readers
{
    public interface IInputReader
    {
        IList<IRenewalItem> Items { get; }
        IList<string> Errors { get; }
        Task ReadInputAsync();
    }
}