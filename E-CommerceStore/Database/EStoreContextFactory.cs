using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceStore.Database
{
    public class EStoreContextFactory : IDesignTimeDbContextFactory<EStoreContext>
    {
        public EStoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EStoreContext>();
            return new EStoreContext(optionsBuilder.UseSqlServer(EStoreContext.ConnectionString).Options);
        }
    }
}
