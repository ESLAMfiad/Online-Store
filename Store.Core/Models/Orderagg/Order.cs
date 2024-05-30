using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Models.Orderagg
{
    public class Order:BaseModel
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, 
            decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId= paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        //public int DelievryMethodId { get; set; } //fk 
        //msh m7tag fk 3shan l entityfw zkya kfaya enha tfhm benhom el3laqa wta5od fk mn delivery t7oto hna
        public DeliveryMethod DeliveryMethod { get; set; }
        //navigational prop[many] lazm t3mlha initialize
        public ICollection<OrderItem> Items { get; set; } =new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        //[NotMapped]
        //public decimal Total { get => SubTotal + DeliveryMethod.Cost; }
        public decimal GetTotal() { return SubTotal + DeliveryMethod.Cost; }

        public string PaymentIntentId { get; set; }
        


    }
}
