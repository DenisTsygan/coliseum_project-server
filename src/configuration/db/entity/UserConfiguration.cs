using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    private readonly AuthorizationOptions _authorizationOptions;
    private readonly IPasswordHasher _passwordHasher;

    public UserConfiguration(AuthorizationOptions authorizationOptions, IPasswordHasher passwordHasher)
    {
        _authorizationOptions = authorizationOptions;
        _passwordHasher = passwordHasher;
    }

    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRoleEntity>(
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<UserEntity>().WithMany().HasForeignKey(u => u.UserId)
            )
            ;
        var hashedPassword = _passwordHasher.Generate(_authorizationOptions.Admin.Password);

        var adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        builder.HasData(new UserEntity
        {
            Id = adminId,
            Email = _authorizationOptions.Admin.UserName,
            PasswordHash = hashedPassword,
            UserName = _authorizationOptions.Admin.UserName,

        });

    }
}
//TODO in other files
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        var adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        builder.HasData(new UserRoleEntity
        {
            UserId = adminId,
            RoleId = (int)Role.Admin // Указываем ID роли
        });

    }
}

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermissionEntity>(
                l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(p => p.PermissionId),
                r => r.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId)
            );

        var roles = Enum.GetValues<Role>()
            .Select(r => new RoleEntity
            {
                Id = (int)r,
                Name = r.ToString()
            });

        builder.HasData(roles);

    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasKey(p => p.Id);
        var permissions = Enum.GetValues<Permission>()
            .Select(p => new PermissionEntity
            {
                Id = (int)p,
                Name = p.ToString()
            });

        builder.HasData(permissions);
    }
}

public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSessionEntity>
{
    public void Configure(EntityTypeBuilder<RefreshSessionEntity> builder)
    {
        builder.HasKey(rs => rs.Id);
    }
}

/*public class TokenConfiguration : IEntityTypeConfiguration<Tokenntity>
{
    public void Configure(EntityTypeBuilder<Tokenntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);
    }
}*/