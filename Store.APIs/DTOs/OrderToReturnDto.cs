using Store.Core.Models.Orderagg;

namespace Store.APIs.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }
        //public int DelievryMethodId { get; set; } //fk 
        //msh m7tag fk 3shan l entityfw zkya kfaya enha tfhm benhom el3laqa wta5od fk mn delivery t7oto hna
        public string DeliveryMethod { get; set; } //name
        public decimal DeliveryMethodCost { get; set; }
        //navigational prop[many] lazm t3mlha initialize
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        //[NotMapped]
        //public decimal Total { get => SubTotal + DeliveryMethod.Cost; }

        public string PaymentIntentId { get; set; } 

    }
}
