using AutoMapper;

public class UserMapperProfile : Profile
{

    public UserMapperProfile()
    {
        CreateMap<UserEntity, User>().ForSourceMember(source => source.PasswordHash, dest => dest.DoNotValidate());
    }
}