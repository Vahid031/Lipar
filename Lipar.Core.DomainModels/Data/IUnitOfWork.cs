using System.Threading.Tasks;

namespace Lipar.Core.DomainModels.Data
{
    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
