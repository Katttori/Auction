using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;
using DAL.Entities;
using DAL.Identity.Entities;

namespace BLL.Infrastructure
{
    public class MapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Category, CategoryDTO>();
            cfg.CreateMap<Product, ProductDTO>();
            cfg.CreateMap<Lot, LotDTO>();
            cfg.CreateMap<User, UserDTO>();
            cfg.CreateMap<ApplicationUser, UserDTO>();
        }
    }
}
