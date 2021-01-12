using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ravency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Products
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, Index.Result.Product>();
            CreateMap<ProductCategory, Index.Result.ProductCategory>();
            CreateMap<Language, Language<Index.Result.Product>>();

            CreateMap<Add.Command, Product>();

            CreateMap<ProductCategory, SelectListItem>()
                .ForMember(d => d.Value, opt => opt.MapFrom(c => c.Id))
                .ForMember(d => d.Text, opt => opt.MapFrom(c => c.Name));

            CreateMap<Language<Add.Command.Product>, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Name, opt => opt.MapFrom(c => c.Data.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(c => c.Data.Description));

            CreateMap<Language<Add.Command.Product>, ProductLocale>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.ProductId, opt => opt.Ignore())
                .ForMember(d => d.LanguageId, opt => opt.MapFrom(c => c.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(c => c.Data.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(c => c.Data.Description));
        }
    }
}
