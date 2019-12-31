using DataCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.API
{
    public interface ICartService
    {
        List<CartDetails> SyncShoppingCart(string userEmail, List<Cart> cartUpdates);
      //  List<CartDetails> GetShoopingCart(string userEmail);
    }
}
