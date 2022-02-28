using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ae_resume_api.Authentication
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<EmployeeEntity> Employees { get; set; } = null!;

        public new async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}