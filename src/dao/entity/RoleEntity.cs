using System.Text.Json.Serialization;

public class RoleEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<PermissionEntity> Permissions { get; set; } = [];// на каждый ендпоинт свое разрешение 

    [JsonIgnore]
    public ICollection<UserEntity> Users { get; set; } = [];//связь многии ко многим

}