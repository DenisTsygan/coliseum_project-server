using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class ServiceDbContext(DbContextOptions<ServiceDbContext> options,
    IOptions<AuthorizationOptions> authOptions

    ) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }

    //public DbSet<PermissionEntity> Permissions { get; set; }

    //public DbSet<Tokenntity> Tokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        //modelBuilder.ApplyConfiguration(new TokenConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));

        base.OnModelCreating(modelBuilder);
    }

}