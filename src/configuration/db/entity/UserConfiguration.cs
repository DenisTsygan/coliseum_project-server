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

public class ElectricityConsumedMounthEntityConfiguration : IEntityTypeConfiguration<ElectricityConsumedMounthEntity>
{
    public void Configure(EntityTypeBuilder<ElectricityConsumedMounthEntity> builder)
    {
        builder.HasKey(ecm => ecm.Id);
        builder.Property(ecm => ecm.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(ecm => ecm.PeriodDate)
                   .IsRequired()
                   .HasMaxLength(7); // Формат "MM-yyyy"

        builder.HasMany(ecm => ecm.ElectricyConsumedDays)
            .WithOne(ecd => ecd.ElectricyConsumedMounth)
            .HasForeignKey(ecd => ecd.MounthId); // Указываем внешний ключ


        var ecmId_1 = Guid.Parse("00000000-0000-0000-0000-100000000000");
        var ecmId_2 = Guid.Parse("00000000-0000-0000-0000-200000000000");
        var ecmId_3 = Guid.Parse("00000000-0000-0000-0000-300000000000");
        var currentMonth = DateTime.Now.ToString("MM-yyyy");
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
        var prevPreviousMonth = DateTime.Now.AddMonths(-2).ToString("MM-yyyy");

        builder.HasData(new ElectricityConsumedMounthEntity
        {
            Id = ecmId_1,
            Name = "Client 1",
            PeriodDate = currentMonth,
            Period = 544, // Часов в месяце (предполагаем полный месяц)
            AllElectricyConsumed = 800.5 // Общее потребление
        },
        new ElectricityConsumedMounthEntity
        {
            Id = ecmId_2,
            Name = "Client 2",
            PeriodDate = previousMonth,
            Period = 744, // Часов в месяце (предполагаем полный месяц)
            AllElectricyConsumed = 1212.5 // Общее потребление
        },
        new ElectricityConsumedMounthEntity
        {
            Id = ecmId_3,
            Name = "Client 3",
            PeriodDate = prevPreviousMonth,
            Period = 744, // Часов в месяце (предполагаем полный месяц)
            AllElectricyConsumed = 730.5 // Общее потребление
        });
    }
}

public class ElectricityConsumedDayEntityConfiguration : IEntityTypeConfiguration<ElectricityConsumedDayEntity>
{
    public void Configure(EntityTypeBuilder<ElectricityConsumedDayEntity> builder)
    {
        builder.HasKey(ecd => ecd.Id);

        builder.HasOne(ecd => ecd.ElectricyConsumedMounth)
            .WithMany(ecm => ecm.ElectricyConsumedDays)
            .HasForeignKey(ecd => ecd.MounthId);
    }
}