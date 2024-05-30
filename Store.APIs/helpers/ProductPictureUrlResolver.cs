using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Models;

namespace Store.APIs.helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}"; //_config de dict 3shan awsl ll appsetting json
            }
            return string.Empty;
        }
    }
}
