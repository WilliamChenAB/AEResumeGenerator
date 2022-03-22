using Microsoft.EntityFrameworkCore;

namespace ae_resume_api.DBContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Add associative table key
            builder.Entity<TemplateSectorsEntity>().
                HasKey(x => new { x.TemplateID, x.TypeID });

            // Add unique name constraints
            //builder.Entity<ResumeEntity>().
            //    HasIndex(r => r.Name).IsUnique();
            builder.Entity<SectorTypeEntity>().
                HasIndex(st => st.Title).IsUnique();
        }

        // Entity tables
        public DbSet<EmployeeEntity> Employee { get; set; } = null!;
        public DbSet<ResumeEntity> Resume { get; set; } = null!;
        public DbSet<SectorEntity> Sector { get; set; } = null!;
        public DbSet<SectorTypeEntity> SectorType { get; set; } = null!;
        public DbSet<TemplateEntity> Resume_Template { get; set; } = null!;
        public DbSet<TemplateSectorsEntity> Template_Type { get; set; } = null!;
        public DbSet<WorkspaceEntity> Workspace { get; set; } = null!;

        public new async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}