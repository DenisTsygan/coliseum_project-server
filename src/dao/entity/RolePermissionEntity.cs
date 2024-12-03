public class RolePermissionEntity // //связует две таблицы RoleEntity многие ко многим PermissionEntity
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

}