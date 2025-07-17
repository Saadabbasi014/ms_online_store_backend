using Core.Entites;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IDatabase _redisDb;

        public CartService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<ShopingCart?> SaveCartAsync(ShopingCart cart)
        {
           var created = await _redisDb.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            if (!created) return null;
            return await GetCartAsync(cart.Id);
        }

        public async Task<ShopingCart?> GetCartAsync(string id)
        {
            string key = $"cart:{id}";
            var value = await _redisDb.StringGetAsync(id);
            if (value.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<ShopingCart>(value);
        }

        public async Task DeleteCartAsync(string id)
        {
            string key = $"cart:{id}";
            await _redisDb.KeyDeleteAsync(id);
        }
    }

}
