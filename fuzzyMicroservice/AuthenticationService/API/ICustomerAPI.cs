using DataCore.Entities;

namespace AuthenticationService.API
{
   public interface ICustomerService
    {
        Customer Authenticate(string email, string password);
        Customer GetById(string id);

        Customer AddUser(Customer user);

        Customer getByEmail(string email);

      //  List<CartDetails> SyncShoppingCart(string userEmail, List<Cart> cartUpdates);
      //  List<CartDetails> GetShoopingCart(string userEmail);
    }
}
