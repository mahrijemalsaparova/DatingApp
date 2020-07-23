using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //source,destination
            CreateMap<User, UserForListDto>()
                 .ForMember(dest => dest.PhotoUrl, opt =>
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                 .ForMember(dest => dest.Age, opt =>
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForDetailedDto>()
                 .ForMember(dest => dest.PhotoUrl, opt =>
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt =>
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotosForDetailedDto>();
            //map UserForUpdateDto to ---->  User 
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            // register kısmı için // userforregister kısımdaki propertiler directly User' a map olur
            CreateMap<UserForRegisterDto, User>();
            
        }
    }
}