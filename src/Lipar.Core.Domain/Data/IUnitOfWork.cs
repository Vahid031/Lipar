using System.Threading.Tasks;

namespace Lipar.Core.Domain.Data
{
    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
