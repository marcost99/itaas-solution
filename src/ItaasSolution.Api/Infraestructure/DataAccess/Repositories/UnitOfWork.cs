using ItaasSolution.Api.Domain.Repositories;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ItaasSolutionDbContext _dbContext;
        public UnitOfWork(ItaasSolutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
