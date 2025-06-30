using ArchitectureDemo.DbContexts;

namespace ArchitectureDemo.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }

    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly ApplicationDbContext context = context;

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
