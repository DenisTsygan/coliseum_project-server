public class UserRepository : IUserRepository
{

    //private readonly Object _context;
    //private readonly Object _mapper;

    public static List<User> users = new List<User>();


    public async Task Add(User user)
    {
        users.Add(user);
        await Task.Delay(100);

    }
    public async Task<User> GetByEmail(string email)
    {
        await Task.Delay(1000);

        //var authUserEntity = await _context.AuthUsers;

        var user = users.Find((user) => user.Email == email);
        if (user == null)
        {
            user = User.Create(Guid.NewGuid(), "123", "new", "not found");
        }
        return user;
    }

    public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
    {
        await Task.Delay(100);

        // Находим пользователя по userId
        var user = users.Find(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception($"User with ID {userId} not found.");
        }

        // Получаем список уникальных разрешений (Permissions) из всех ролей пользователя
        var permissions = user.Roles
            .SelectMany(r => r.Permissions) // Извлекаем все Permissions из каждой роли
            .Select(p => (Permission)p.Id) // Преобразуем Id в Permission
            .ToHashSet(); // Преобразуем в HashSet для уникальности

        return permissions;
    }

    public async Task<List<User>> GetList()
    {
        await Task.Delay(100);

        return users;
    }
}