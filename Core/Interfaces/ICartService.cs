using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICartService
    {
        Task<ShopingCart?> SaveCartAsync(ShopingCart cart);
        Task<ShopingCart?> GetCartAsync(string userId);
        Task DeleteCartAsync(string userId);
    }
}
