using StockManagement.CCC.Entities;
using StockManagement.CCC.Models;
using AutoMapper;

namespace StockManagement.CCC.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Product, ProductResponse>();
            CreateMap<PriceHistory, PriceHistoryResponse>();
        }
    }
}
