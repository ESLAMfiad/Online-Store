using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Models.Orderagg
{
    public class OrderItem :BaseModel
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrdered product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }

        public  ProductItemOrdered Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
