using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.Core.Models;
using Store.Core.Services;
using Stripe;

namespace Store.APIs.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "whsec_8f1de19d9f5fb2f23eb68455806a102eb0239e0ca2f00f3e8f41d5e81cea19b3";


        public PaymentsController(IPaymentService paymentService,IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        //creat or update endpoint
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateUpdatePaymentInent(string basketId)
        {
          var customerBasket= await _paymentService.CreateUpdatePaymentIntent(basketId);
            if (customerBasket is null) return BadRequest(new ApiResponse(400,"there is a problem with your basket"));
            var mappedBasket = _mapper.Map<CustomerBasket,CustomerBasketDto>(customerBasket);
            return Ok(mappedBasket);
        }

        [HttpPost("webhook")] //baseurl/api/payments/webhook
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                   await  _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id,false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, true);

                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        } 

        
    }
}
