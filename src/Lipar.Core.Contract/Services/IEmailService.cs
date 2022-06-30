using Lipar.Core.Domain.Entities;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Services;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}


