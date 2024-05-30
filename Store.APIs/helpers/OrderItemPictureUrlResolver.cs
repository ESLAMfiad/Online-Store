using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Models.Orderagg;

namespace Store.APIs.helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}"; //_config de dict 3shan awsl ll appsetting json
            }
            return string.Empty;
        }
    }
}
