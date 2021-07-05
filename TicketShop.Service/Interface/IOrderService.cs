using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.Identity;

namespace TicketShop.Service.Interface {
    public interface IOrderService {
        List<Order> getAllOrders(TicketShopUser user);
        Order getOrderDetails(Guid? id);
    }
}
