using Notifications.Core.Models;
using System.Threading.Tasks;

namespace Notifications.Core.Writers
{
    public interface IOutputWriter
    {
        Task WriteAsync(IRenewalItem renewalItem, string content);
        Task WriteErrorLogAsync(string content);
    }
}