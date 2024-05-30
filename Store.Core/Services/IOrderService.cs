using Store.Core.Models.Orderagg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketID, int deliveryMethodId, Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);
        Task<Order> GetOrdersByIdForSpecificUserAsync(string buyerEmail, int orderId);
    }
}
