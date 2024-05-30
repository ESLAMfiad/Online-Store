using Store.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repository
{
    public interface IBasketRepository
    {
        //get basket
        Task<CustomerBasket?> GetBasketAsync(string BasketId);
        //update/create basket
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket);
        //delete basket
        Task<bool> DeleteBasketAsync(string BasketId);

    }
}
