public class UserService
{

    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider
        )
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }
    public async Task Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);
        await _userRepository.Add(user);
    }

    public async Task<string> Login(string email, string password)
    {
        await Task.Delay(123);
        var user = _userRepository.GetByEmail(email);
        if (user == null)
        {
            throw new Exception("Failed login. Not fount user");
        }
        var result = _passwordHasher.Verify(password, user.Result.PasswordHash);
        if (result == false)
        {
            throw new Exception("Failed login. Password not correct");
        }
        var token = _jwtProvider.GenerateToken(user.Result);

        return token;
    }

    public async Task<List<User>> GetList()
    {
        await Task.Delay(123);
        return await _userRepository.GetList();
    }
}