using Microsoft.EntityFrameworkCore;

namespace ae_resume_api.DBContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}

        public DbSet<EmployeeEntity> Employees { get; set; } = null!;

        //public new async Task<int> SaveChanges()
        //{
        //    return await base.SaveChangesAsync();
        //}
    }
}