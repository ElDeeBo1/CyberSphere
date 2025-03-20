using AutoMapper;
using CyberSphere.BLL.DTO.ArticleDTO;
using CyberSphere.BLL.DTO.CourseDTO;
using CyberSphere.BLL.DTO.LessonDTO;
using CyberSphere.BLL.DTO.LevelDTO;
using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateArticleDTO, Article>().ReverseMap();
            CreateMap<CreateArticleDTO, Article>()
          .ForMember(dest => dest.ImgURL, opt => opt.MapFrom(src => src.Image));

            CreateMap<Article, GetArticleByIdDTO>()
                .ForMember(dst => dst.Image, opt => opt.MapFrom(src => src.ImgURL));
  
            CreateMap<Article,GetAllArticlesDTO>()
                .ForMember(dst => dst.Image,opt => opt.MapFrom(src => src.ImgURL));

            CreateMap<UpdateArticleDTO, Article>()
                .ForMember(dst => dst.ImgURL, opt => opt.MapFrom(src => src.Image)).ReverseMap();

                
            CreateMap<Lesson_ModelDTO,Lesson>().ReverseMap();
            CreateMap<CreateLessonDTO,Lesson>().ReverseMap();
            CreateMap<UpdateLessonDTO, Lesson>().ReverseMap();
            CreateMap<GetLessonByIdDTO, Lesson>().ReverseMap();
            CreateMap<Lesson, GetAllLessonsByCourseIdDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.VideoURL, opt => opt.MapFrom(src => src.VideoURL))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));



            CreateMap<CreateCourseDTO,Course>().ReverseMap();
            CreateMap<UpdateCourseDTO,Course>().ReverseMap();
            CreateMap<Course, GetAllCoursesDTO>()
                .ForMember(dst =>dst.LevelName,opt => opt.MapFrom(src => src.Title));
            CreateMap<Course, GetCourseByIdDTO>()
             .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.Title))  
             .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons));
            CreateMap<Course_ModelDTO, Course>().ReverseMap();

            CreateMap<CreateLevelDTO, Level>().ReverseMap();
            CreateMap<UpdateLevelDTO, Level>().ReverseMap();
            CreateMap<Level,GetLevelByIdDTO>()
                .ForMember(dst =>dst.ParentLevelId,opt => opt.MapFrom(src => src.ParentLevelId));
            CreateMap<Level, GetAllLevelsDTO>();
        }


    }}
