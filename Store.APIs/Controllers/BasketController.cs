using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.Core.Models;
using Store.Core.Repository;
using System.Reflection.Metadata.Ecma335;

namespace Store.APIs.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketrepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketrepo,IMapper mapper)
        {
           _basketrepo = basketrepo;
            _mapper = mapper;
        }
        //Get or recreate
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket (string BasketId)
        {
            var Basket= await _basketrepo.GetBasketAsync(BasketId);
            return Basket is null ? new CustomerBasket(BasketId) : Ok(Basket);

        }
        //Update or create new basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto Basket)
        {
           var mappedbasket= _mapper.Map<CustomerBasketDto,CustomerBasket>(Basket);
           var createdorUpdatedBasket= await _basketrepo.UpdateBasketAsync(mappedbasket);
            if (createdorUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(createdorUpdatedBasket);
            //return createdorUpdatedBasket is null? BadRequest(new ApiResponse(400)) : Ok(createdorUpdatedBasket); 
        }

        //delete
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketrepo.DeleteBasketAsync(BasketId);
        }

    }
}
