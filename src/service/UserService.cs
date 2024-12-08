public class UserService
{

    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshSessionRepository _refreshSessionRepository;

    private readonly IJwtProvider _jwtProvider;

    public UserService(IUserRepository userRepository,
        IRefreshSessionRepository refreshSessionRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider
        )
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _refreshSessionRepository = refreshSessionRepository;
        _jwtProvider = jwtProvider;
    }
    public async Task<User> Register(string userName, string email, string password, int roleId)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = UserEntity.Create(Guid.NewGuid(), userName, hashedPassword, email);
        return await _userRepository.Add(user, roleId);
    }

    public async Task<UserServiceLoginResponce> Login(string email, string password, string userAgent, string fingerprint, string ip)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user == null)
        {
            throw new Exception("Failed login. Not fount user");
        }
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
        {
            throw new Exception("Failed login. Password not correct");
        }


        var refreshToken = await _jwtProvider.GenerateRefreshToken(user.Id, userAgent, fingerprint, ip);

        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var res = new UserServiceLoginResponce(AccessToken: accessToken, RefreshToken: refreshToken);
        return res;
    }

    public async Task<UserServiceLoginResponce> RefreshToken(string token, string fingerprint, string userAgent, string ip)
    {
        var tokenGuid = _jwtProvider.GetClaimFromToken(token, CustomClaims.RefreshSessionId);
        var oldRefreshSession = await _refreshSessionRepository.GetByRefreshToken(tokenGuid);
        if (oldRefreshSession is null)
        {
            throw new Exception("Not avaible session");
        }
        await _refreshSessionRepository.Delete(oldRefreshSession);

        //TODO verify 

        var nowTime = DateTime.UtcNow;
        if (nowTime > oldRefreshSession.ExpiresIn)
        {
            throw new Exception("Sessin(refresh token) expired ");
        }
        if (oldRefreshSession.FingerPrint != fingerprint)
        {
            throw new Exception("NVALID_REFRESH_SESSION fingerprint");
        }
        //---

        var user = await _userRepository.GetById(oldRefreshSession.UserId);
        if (user == null)
        {
            throw new Exception("Failed refreshtoken. Not fount user");
        }

        var refreshTokenNew = await _jwtProvider.GenerateRefreshToken(user.Id, userAgent, fingerprint, ip);

        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var res = new UserServiceLoginResponce(AccessToken: accessToken, RefreshToken: refreshTokenNew);
        return res;
    }

    public async Task Logout(string token)
    {
        var tokenGuid = _jwtProvider.GetClaimFromToken(token, CustomClaims.RefreshSessionId);
        var oldRefreshSession = await _refreshSessionRepository.GetByRefreshToken(tokenGuid);
        if (oldRefreshSession is null)
        {
            throw new Exception("Not avaible session");
        }
        await _refreshSessionRepository.Delete(oldRefreshSession);
    }

    public async Task LogoutByRefres(string rsid)
    {
        await _refreshSessionRepository.DeleteById(rsid);
    }

    public async Task LogoutAllSession(string token)
    {
        var tokenGuid = _jwtProvider.GetClaimFromToken(token, CustomClaims.RefreshSessionId);
        var oldRefreshSession = await _refreshSessionRepository.GetByRefreshToken(tokenGuid);
        if (oldRefreshSession is null)
        {
            throw new Exception("Not avaible session");
        }
        await _refreshSessionRepository.DeleteAllByUserId(oldRefreshSession.UserId);
    }

    public async Task<List<User>> GetList()
    {
        await Task.Delay(123);
        return await _userRepository.GetList();
    }
}