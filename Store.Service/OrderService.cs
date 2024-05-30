using Store.Core;
using Store.Core.Models;
using Store.Core.Models.Orderagg;
using Store.Core.Repository;
using Store.Core.Services;
using Store.Core.Specifications.order_spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketID, int deliveryMethodId, Address ShippingAddress)
        {
           //1.get basket from basket repo
           var Basket= await _basketRepository.GetBasketAsync(basketID);
            //2.get selected items at basket from product repo
            var orderItems= new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered= new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, product.Price);
                    orderItems.Add(OrderItem);
                }
            }
            //3.calc subtotal
            var SubtTotal = orderItems.Sum(item => item.Quantity * item.Price);
            //4.get delivery method from delivery method repo
            var DeliveryMethod= await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            //5.create order
            var spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var exOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(exOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(exOrder);
                await _paymentService.CreateUpdatePaymentIntent(basketID);
            }

            var Order= new Order(buyerEmail,ShippingAddress,DeliveryMethod,orderItems,SubtTotal,Basket.PaymentIntentId);
            //6.add order localy
            await _unitOfWork.Repository<Order>().Add(Order);
            //7.save order to database[toDto]
            var res= await _unitOfWork.CompleteAsync();
            if(res <=0) return null;

            return Order;
        }

        public async Task<Order> GetOrdersByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification(buyerEmail, orderId);
            var orders = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec= new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
