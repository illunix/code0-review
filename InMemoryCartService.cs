using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Services
{
    public interface IInMemoryCartService
    {
        Task<Cart> GetCart(Guid userId);
        Task UpdateCart(Guid userId, Cart cart);
        Task DeleteCart(Guid userId);
    }

    public class InMemoryCartService : IInMemoryCartService
    {
        private readonly IMemoryCache _cache;

        public InMemoryCartService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<Cart> GetCart(Guid userId)
            => Task.FromResult(_cache.Get<dynamic>(GetKey(userId)));

        public Task UpdateCart(Guid userId, Cart cart)
        {
            _cache.Set(GetKey(userId), cart);

            return Task.CompletedTask;
        }

        public Task DeleteCart(Guid userId)
        {
            _cache.Remove(GetKey(userId));

            return Task.CompletedTask;
        }

        private static string GetKey(Guid userId)
            => $"users-carts:{userId}";
    }
}
