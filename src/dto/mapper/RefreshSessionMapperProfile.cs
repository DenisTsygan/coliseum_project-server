using AutoMapper;

public class RefreshSessionMapperProfile : Profile
{

    public RefreshSessionMapperProfile()
    {
        CreateMap<RefreshSessionEntity, RefreshSession>()
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}