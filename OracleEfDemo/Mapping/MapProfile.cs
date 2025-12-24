using AutoMapper;
using OracleEfDemo.Dtos;
using OracleEfDemo.Models;

namespace OracleEfDemo.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Categories, CategoriesDto>().ReverseMap();
            CreateMap<Customers, CustomersDto>().ReverseMap();
            CreateMap<OrderItems, OrderItemsDto>().ReverseMap();
            CreateMap<Orders, OrdersDto>().ReverseMap();
            CreateMap<Products, ProductsDto>().ReverseMap();
            CreateMap<StockLog, StockLogDto>().ReverseMap();
        }
    }
}
