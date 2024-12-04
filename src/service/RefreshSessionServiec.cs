using AutoMapper;

public class RefreshSessionService
{

    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshSessionRepository _refreshSessionRepository;


    public RefreshSessionService(IUserRepository userRepository,
        IRefreshSessionRepository refreshSessionRepository,
        IMapper mapper
        )
    {
        _userRepository = userRepository;
        _refreshSessionRepository = refreshSessionRepository;
        _mapper = mapper;
    }

    public async Task<List<RefreshSession>> GetListSessions()
    {

        var sessions = await _refreshSessionRepository.GetList();

        var userIds = sessions.Select(s => s.UserId).ToHashSet();

        var users = await _userRepository.GetListByIds(userIds);

        var mappedSessions = sessions.Select(session =>
        {
            var refreshSession = _mapper.Map<RefreshSession>(session);
            if (users.TryGetValue(session.UserId, out var userEntity))
            {
                refreshSession.User = _mapper.Map<User>(userEntity);
            }
            return refreshSession;
        }).ToList();

        return mappedSessions;
    }
}