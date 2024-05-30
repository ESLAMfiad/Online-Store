using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.APIs.helpers;
using Store.Core;
using Store.Core.Models;
using Store.Core.Repository;
using Store.Core.Specifications;

namespace Store.APIs.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        

        public ProductsController( IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }
        //get all products
        [Authorize] // (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)  momkn abdlob "bearer"
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParam Params)
        {
            var Spec = new ProductWithBrandAndTypeSpec(Params);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            //var returnedObject = new Pagination<ProductToReturnDto>()
            //{
            //    PageIndex=Params.PageIndex,
            //    PageSize=Params.PageSize,
            //    Data=mappedProducts
            //};  //dol bdl str el return ok l t7t ay treqa mnhom tnf3
            var CountSpec= new ProductWithFilterationForCountAsync(Params);
            var Count= await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex,Params.PageSize,mappedProducts,Count));
        }
        //get products by id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] //statuscode de enum feha kol anwa3 el errors
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpec(id);
            var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            if (Product is null) return NotFound(new ApiResponse(404));
            var mappedProducts = _mapper.Map<Product, ProductToReturnDto>(Product);

            return Ok(mappedProducts);
        }

        //get all types
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
        //get all brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
    }
}
