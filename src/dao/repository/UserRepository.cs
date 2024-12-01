public class UserRepository : IUserRepository
{

    //private readonly Object _context;
    //private readonly Object _mapper;

    public static List<User> users = new List<User>();


    public async Task Add(User user)
    {
        users.Add(user);
        Console.WriteLine("Add user list:");
        users.ForEach(i => Console.Write("{0}\t", i));
        //await _context
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
}