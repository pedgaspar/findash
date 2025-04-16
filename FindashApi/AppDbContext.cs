using Findash.Users;
using Microsoft.EntityFrameworkCore;

namespace Findash;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    // public DbSet<Employee> Employees { get; set; }
    // public DbSet<Benefit> Benefits { get; set; }
    // public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<EmployeeBenefit>()
        //     .HasIndex(eb => new { eb.EmployeeId, eb.BenefitId })
        //     .IsUnique();
        //
        // modelBuilder.Entity<Employee>()
        //     .HasMany(e => e.Benefits)
        //     .WithOne(eb => eb.Employee)
        //     .HasForeignKey(eb => eb.EmployeeId);
        //
        // modelBuilder.Entity<EmployeeBenefit>()
        //     .HasKey(eb => new { eb.EmployeeId, eb.BenefitId })
        //     .HasName("PK_EmployeeBenefit");
    }
    
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "God";
                entry.Entity.CreatedOn = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = "God";
                entry.Entity.LastModifiedOn = DateTime.UtcNow;
            }
        }
    }
}