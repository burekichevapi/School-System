using AutoMapper;
using FinalProjectSPC.DTO;
using FinalProjectSPC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MappingConfig());
            });
        }
    }

    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();

        }
    }
}
