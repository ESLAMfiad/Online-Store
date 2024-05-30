using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Models;
using Store.Core.Models.identity;
using Store.Core.Models.Orderagg;
using IdentityAddress = Store.Core.Models.identity.Address;
using OrderAddress = Store.Core.Models.Orderagg.Address;


namespace Store.APIs.helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>().ForMember(d=>d.ProductType,o=>o.MapFrom(S=>S.ProductType.Name))
                .ForMember(d => d.ProductBrand, o => o.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<AddressDto, OrderAddress>();

            CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
