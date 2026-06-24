using AutoMapper;
using Pixora.BL.DTOs;
using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UploadPhotoDto, Photo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.FileSizeBytes, opt => opt.MapFrom(src => src.FileSizeBytes))
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.PhotoHashtags, opt => opt.Ignore());

            CreateMap<Photo, PhotoDto>()
                .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => src.Author.Email))
                .ForMember(dest => dest.Hashtags, opt => opt.MapFrom(src =>
                    src.PhotoHashtags.Select(ph => ph.Hashtag.Name).ToList()));

            CreateMap<ApplicationUser, UserDto>();

            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserActionLog, UserActionLogDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => src.ActionType.ToString()));
        }
    }
}
