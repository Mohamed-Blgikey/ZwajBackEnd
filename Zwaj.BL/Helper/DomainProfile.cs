using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.DTOs;
using Zwaj.DAL.Entity;
using Zwaj.DAL.Extend;

namespace Zwaj.BL.Helper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<User, UserForListDTO>().ForMember(src => src.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(a => a.IsMain).Url); }); ;
            CreateMap<User, UserForDetailsDTO>().ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(a => a.IsMain).Url); });

            CreateMap<Photo, PhotoForDetailsDTO>();

            CreateMap<UserForUpdateDTO, User>();
            CreateMap<PhotoForUserDTO, Photo>();
        }
    }
}
