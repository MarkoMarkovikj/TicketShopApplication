using TicketShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Service.Interface {
    public interface IShoppingCartService {
        ShoppingCartDTO GetShoppingCartInfo(String userId);

        bool DeleteProductFromShoppingCart(String userId, Guid id);

        bool OrderNow(String userId);
    }
}
