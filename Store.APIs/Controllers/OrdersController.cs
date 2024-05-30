using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.Core;
using Store.Core.Models.Orderagg;
using Store.Core.Services;
using Store.Service;
using System.Security.Claims;

namespace Store.APIs.Controllers
{
   
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService,IMapper mapper,IUnitOfWork  unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        //create order
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] //post => baseurl/api/orders
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail= User.FindFirstValue(ClaimTypes.Email);
            var mappedaddress= _mapper.Map<AddressDto,Address>(orderDto.shipToAddress);
            var Order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, mappedaddress);
            if (Order is null) return BadRequest(new ApiResponse(400,"there is a problem with your order"));
            return Ok(Order);
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser() 
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders= await _orderService.GetOrdersForSpecificUserAsync(buyerEmail);
            if (orders is null) return NotFound(new ApiResponse(404, "there is no orders for this user"));
            var mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrders);
        }


        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>>GetOrderByIdForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order= await _orderService.GetOrdersByIdForSpecificUserAsync(buyerEmail,id);
            if (order is null) return NotFound(new ApiResponse(404, $"there is no order with id={id} for this user"));
            var mappedorders=_mapper.Map<Order,OrderToReturnDto>(order);
            return Ok(mappedorders);
        }



        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods= await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(deliveryMethods);
        }
    }
}
