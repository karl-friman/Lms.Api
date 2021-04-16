using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap();
            CreateMap<Course, CourseModulesDto>()
                .ForMember(dest => dest.Modules,
                opt => opt.MapFrom(src => src.Modules.ToList()))
                .ReverseMap();
        }

    }
}
