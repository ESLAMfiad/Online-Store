using Store.Core.Models.Orderagg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.order_spec
{
    public class OrderWithPaymentIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string paymentIntentId):base(O=>O.PaymentIntentId==paymentIntentId)
        {
                
        }
    }
}
