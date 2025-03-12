using AutoMapper;
using CyberSphere.BLL.DTO.ArticleDTO;
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

            //CreateMap<Article, UpdateArticleDTO>()
            //    .ForMember(dst => dst.Image, opt => opt.MapFrom(src => src.ImgURL));


            CreateMap<UpdateArticleDTO, Article>()
                .ForMember(dst => dst.ImgURL, opt => opt.MapFrom(src => src.Image)).ReverseMap();


            //CreateMap<UpdateArticleDTO,Article>().ReverseMap();
                


        }
    }
}
