using System.Threading.Tasks;

namespace Lipar.Core.Contract.Data;

public interface IInBoxEventRepository
{
    bool AllowReceive(string messageId, string fromService);
    Task Receive(string messageId, string fromService);
}


