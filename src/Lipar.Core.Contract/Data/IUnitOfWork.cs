using System.Threading.Tasks;

namespace Lipar.Core.Contract.Data
{
    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
