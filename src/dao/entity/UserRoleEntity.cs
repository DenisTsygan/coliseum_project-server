public class UserRoleEntity//связует две таблицы UserEntity многие ко многим RoleEntity
{

    public Guid UserId { get; set; }

    public int RoleId { get; set; }
}