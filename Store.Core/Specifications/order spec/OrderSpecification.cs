using Store.Core.Models.Orderagg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.order_spec
{
    public class OrderSpecification :BaseSpecifications<Order>
    {
        public OrderSpecification(string email):base(O=>O.BuyerEmail==email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O=>O.OrderDate);
        }

        public OrderSpecification(string email,int orderId):base(O=>O.BuyerEmail==email && O.Id ==orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
