using Findash.Domain.Accounts;
using Findash.Users;
using Microsoft.EntityFrameworkCore;

namespace Findash;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
   
    // public DbSet<Benefit> Benefits { get; set; }
    // public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccount>()
            .HasIndex(eb => new { eb.UserId, eb.AccountId })
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.Accounts)
            .WithOne(eb => eb.User)
            .HasForeignKey(eb => eb.UserId);
        
        modelBuilder.Entity<UserAccount>()
            .HasKey(eb => new { eb.UserId, eb.AccountId })
            .HasName("PK_EmployeeBenefit");
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