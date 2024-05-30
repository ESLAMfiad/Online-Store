using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using Store.Core.Models;
using Store.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Store.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis) //ask clr for object from class implement interface IconMul..
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
           return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket= await _database.StringGetAsync(BasketId);
            //if(Basket.IsNull) return null;
            //else
            //    var returnedBasket= JsonSerializer.Deserialize<CustomerBasket>(Basket); //msh hynf3 a5znha f var fa momkn a3mlha return 3la tol aw eltreqa eltanya
            return Basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonBasket = JsonSerializer.Serialize(Basket);
           var createdorUpdated= await _database.StringSetAsync(Basket.Id, JsonBasket,TimeSpan.FromDays(1));
            if (!createdorUpdated) return null;
            return await GetBasketAsync(Basket.Id);
        }
    }
}
