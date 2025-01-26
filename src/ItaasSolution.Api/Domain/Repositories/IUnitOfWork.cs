using System.Threading.Tasks;

namespace ItaasSolution.Api.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
