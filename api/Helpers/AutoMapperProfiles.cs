using api.DTOs;
using api.Entities;
using api.Extensions;
using AutoMapper;

namespace api.Helpers;

public class AutoMapperProfiles : Profile
{
    //constructor use ctor for creating it
    public AutoMapperProfiles()
    {
        //from appuser to member dto
        CreateMap<AppUser, MemberDto>()
        .ForMember(d=>d.Age , o => o.MapFrom(s=>s.DateOfBirth.CalculateAge()))
        .ForMember(d=>d.PhotoUrl , 
        o=>o.MapFrom(s=>s.Photos.FirstOrDefault(x=>x.IsMain)!.Url));

        //from photo to photoDto
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, AppUser>();
    }

}
