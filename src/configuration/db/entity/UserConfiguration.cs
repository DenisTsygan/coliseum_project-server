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

        builder.Property(ecm => ecm.PeriodDate)
                   .IsRequired()
                   .HasMaxLength(7); // Формат "MM-yyyy"

        builder.HasMany(ecm => ecm.ElectricyConsumedDays)
            .WithOne(ecd => ecd.ElectricyConsumedMounth)
            .HasForeignKey(ecd => ecd.MounthId); // Указываем внешний ключ

        builder.HasOne(ecm => ecm.Client)
            .WithMany(c => c.ElectricityConsumedMounthEntities)
            .HasForeignKey(ecm => ecm.ClientId);

        var clientId_1 = Guid.Parse("c0000000-0000-0000-0000-000000000001");
        var clientId_2 = Guid.Parse("c0000000-0000-0000-0000-000000000002");
        var clientId_3 = Guid.Parse("c0000000-0000-0000-0000-000000000003");
        var clientsId = new Guid[3] { clientId_1, clientId_2, clientId_3 };

        var currentMonth = DateTime.Now.ToString("MM-yyyy");
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
        var prevPreviousMonth = DateTime.Now.AddMonths(-2).ToString("MM-yyyy");
        var mounthes = new string[3] { currentMonth, previousMonth, prevPreviousMonth };
        Random random = new Random();

        for (int client_count = 0; client_count < 3; client_count++)
        {
            for (int mounth_count = 0; mounth_count < 3; mounth_count++)
            {
                builder.HasData(new ElectricityConsumedMounthEntity
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-" + (client_count + 1) + "" + (mounth_count + 1) + "0000000000"),
                    ClientId = clientsId[client_count],
                    PeriodDate = mounthes[mounth_count],
                    Period = 30 * 24 + 0.1m,
                    AllElectricyConsumed = Math.Round(random.NextDouble() * (2000 - 1) + 1, 2)
                });
            }
        }
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

        var clientId_1 = Guid.Parse("c0000000-0000-0000-0000-000000000001");
        var clientId_2 = Guid.Parse("c0000000-0000-0000-0000-000000000002");
        var clientId_3 = Guid.Parse("c0000000-0000-0000-0000-000000000003");
        var clientsId = new Guid[3] { clientId_1, clientId_2, clientId_3 };

        var currentMonth = DateTime.Now.ToString("MM-yyyy");
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
        var prevPreviousMonth = DateTime.Now.AddMonths(-2).ToString("MM-yyyy");
        var mounthes = new string[3] { currentMonth, previousMonth, prevPreviousMonth };
        Random random = new Random();

        for (int client_count = 0; client_count < 3; client_count++)
        {
            for (int mounth_count = 0; mounth_count < 3; mounth_count++)
            {
                var days = new List<ElectricityConsumedDayEntity>();
                for (int count_day = 0; count_day < 30; count_day++)
                {
                    var hours = new double[24];
                    for (int k = 0; k < 24; k++)
                    {
                        hours[k] = Math.Round(random.NextDouble() * (10 - 1) + 1, 2);
                    }
                    days.Add(new ElectricityConsumedDayEntity
                    {
                        Id = Guid.NewGuid(),
                        Day = count_day + 1,
                        AllElectricyConsumed = Math.Round(random.NextDouble() * (200 - 1) + 1, 2),
                        ElectricyConsumedHours = hours,
                        MounthId = Guid.Parse("00000000-0000-0000-0000-" + (client_count + 1) + "" + (mounth_count + 1) + "0000000000"),
                    });
                }

                builder.HasData(days);
                days.Clear();
            }
        }
    }
}

public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasKey(ecd => ecd.Id);

        builder.HasMany(c => c.ElectricityConsumedMounthEntities)
            .WithOne(ecm => ecm.Client)
            .HasForeignKey(ecm => ecm.ClientId);

        var clientId_1 = Guid.Parse("c0000000-0000-0000-0000-000000000001");
        var clientId_2 = Guid.Parse("c0000000-0000-0000-0000-000000000002");
        var clientId_3 = Guid.Parse("c0000000-0000-0000-0000-000000000003");
        builder.HasData(new ClientEntity
        {
            Id = clientId_1,
            Name = "Client 1",
        }, new ClientEntity
        {
            Id = clientId_2,
            Name = "Client 2",
        }, new ClientEntity
        {
            Id = clientId_3,
            Name = "Client 3",
        });
    }
}