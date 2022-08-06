using Microsoft.EntityFrameworkCore;

namespace E_CommerceStore.Database
{
    public class EStoreContext : DbContext
    {

        public EStoreContext(DbContextOptions<EStoreContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
