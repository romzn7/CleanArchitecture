using CleanArchitecture.Services.Shared.Application.Mapping;
namespace CleanArchitecture.Services.Person.API.Mapping;
public class MappingProfile : BaseMappingProfile
{
    public MappingProfile()
       : base(typeof(MappingProfile).Assembly)
    {
    }
}