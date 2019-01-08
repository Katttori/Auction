using AutoMapper;
using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB.Models;

namespace WEB.App_Start
{
    public class MapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LotDTO, LotModel>();
            cfg.CreateMap<ProductModel, ProductDTO>()
                .ForMember(mem => mem.IsConfirmed, opt => opt.Ignore())
                .ForMember(mem => mem.IsSold, opt => opt.Ignore())
                .ForMember(mem => mem.Owner, opt => opt.Ignore())
                .ForMember(mem => mem.Category, opt => opt.Ignore());
            cfg.CreateMap<ProductDTO, ProductModel>()
                .ForMember(mem => mem.Category, map => map.MapFrom(src => src.Category.Name))
                .ForMember(mem => mem.Owner, map => map.MapFrom(src => src.Owner.Name));
            cfg.CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
            cfg.CreateMap<UserDTO, UserModel>().ReverseMap();   
        }

        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                Configure(cfg);
                BLL.Infrastructure.MapperConfig.Configure(cfg);
            });
        }
    }
}