using AutoMapper;
using EventPlanner.Application.Services.EventService.Dto;
using EventPlanner.Domain.Entity;

namespace EventPlanner.Application.mappers;
public class EventMappingProfile: Profile
{
    public EventMappingProfile() {
        CreateMap<CreateUpdateEventRequestDto, Event>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<CreateUpdateRequestInvites, Invite>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
