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

            CreateMap<OrderItems, OrderItemsDto>()
                .ForMember(dest => dest.ProductsDto, opt => opt.MapFrom(src => src.Products));

            CreateMap<OrderItemsDto, OrderItems>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductsDto));

            CreateMap<Orders, OrdersDto>()
            .ForMember(dest => dest.OrderItemsDto, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.CustomersDto, opt => opt.MapFrom(src => src.Customers));

            CreateMap<OrdersDto, Orders>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemsDto));

            CreateMap<Products, ProductsDto>().ReverseMap();
            CreateMap<StockLog, StockLogDto>().ReverseMap();
            CreateMap<UserApp, UserDto>().ReverseMap();
        }
    }
}
