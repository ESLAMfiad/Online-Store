using Store.Core.Models;
using Store.Core.Models.Orderagg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services
{
    public interface IPaymentService
    {
        //func to  create or update payment intent
        Task<CustomerBasket?> CreateUpdatePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag);

    }
}
