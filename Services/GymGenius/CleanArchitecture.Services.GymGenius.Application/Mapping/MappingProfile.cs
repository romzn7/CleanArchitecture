using CleanArchitecture.Services.Shared.Application.Mapping;

namespace CleanArchitecture.Services.GymGenius.Application.Mapping;
public class MappingProfile : BaseMappingProfile
{
    public MappingProfile()
       : base(typeof(MappingProfile).Assembly)
    {
    }
}
