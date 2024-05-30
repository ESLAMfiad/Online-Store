using Microsoft.Extensions.Configuration;
using Store.Core;
using Store.Core.Models;
using Store.Core.Models.Orderagg;
using Store.Core.Repository;
using Store.Core.Services;
using Store.Core.Specifications.order_spec;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Core.Models.Product;

namespace Store.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateUpdatePaymentIntent(string basketId)
        {
            //secret eky
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            //get basket
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;
            var shippingprice = 0M; //decimal
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliverymethod=  await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingprice = deliverymethod.Cost;
            }

            //total =subtot+delivmethod cost
            if(basket.Items.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }
            var subtotal=basket.Items.Sum(x=>x.Price*x.Quantity);

            //create payment intent
            var service =new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId)) // create 
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subtotal *100 + shippingprice*100),
                    Currency= "usd",
                    PaymentMethodTypes= new List<string>() {"card"}
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId=paymentIntent.Id;
                basket.ClientSecret=paymentIntent.ClientSecret;
            }
            else //update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount= (long)(subtotal * 100 + shippingprice * 100),                   
                };
                paymentIntent= await service.UpdateAsync(basket.PaymentIntentId, options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentRecieved;
            }
            else
            {
                order.Status= OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
