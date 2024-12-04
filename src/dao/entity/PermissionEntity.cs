using System.Text.Json.Serialization;

public class PermissionEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<RoleEntity> Roles { get; set; } = [];

}