using Lipar.Core.Domain.Entities;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Utilities
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
