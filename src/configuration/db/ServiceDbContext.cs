using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class ServiceDbContext(DbContextOptions<ServiceDbContext> options,
    IOptions<AuthorizationOptions> authOptions,
    IPasswordHasher passwordHasher

    ) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }

    public DbSet<RefreshSessionEntity> RefreshSessions { get; set; }

    public DbSet<ElectricityConsumedMounthEntity> ElectricityConsumedMounthEntities { get; set; }

    public DbSet<ElectricityConsumedDayEntity> ElectricityConsumedDayEntities { get; set; }

    public DbSet<ClientEntity> Clients { get; set; }

    //public DbSet<PermissionEntity> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshSessionConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration(authOptions.Value, passwordHasher));

        modelBuilder.ApplyConfiguration(new ElectricityConsumedMounthEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ElectricityConsumedDayEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ClientEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }

}