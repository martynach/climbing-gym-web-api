using AutoMapper;
using dot_net_api.Dtos;
using dot_net_api.Entities;

namespace dot_net_api.Mappers;

public class MappingProfile: Profile
{

    public MappingProfile()
    {
        CreateMap<ClimbingGym, ClimbingGymDto>()
            .ForMember(dto => dto.City, config => config.MapFrom(s => s.Address.City))
            .ForMember(dto => dto.PostalCode, config => config.MapFrom(s => s.Address.PostalCode))
            .ForMember(dto => dto.Street, config => config.MapFrom(s => s.Address.Street));

        CreateMap<ClimbingRoute, ClimbingRouteDto>();

        CreateMap<CreateClimbingGymDto, ClimbingGym>()
            .ForMember(d => d.Address,
                config => config.MapFrom(dto => new Address()
                    { City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }));

    }
    
}