using StockManagement.CCC.Entities;
using StockManagement.CCC.Models;
using AutoMapper;

namespace StockManagement.CCC.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Product, ProductResponse>();
            CreateMap<PriceHistory, PriceHistoryResponse>();
            CreateMap<Product, UpdatePriceResponse>().
                ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.Price));
            CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.PriceHistories, opt => opt.MapFrom(src => src.PriceHistories));
            CreateMap<PriceHistory, PriceHistoryResponse>();
        }
    }
}
