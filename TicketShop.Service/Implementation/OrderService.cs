using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.Identity;
using TicketShop.Repository.Interface;
using TicketShop.Service.Interface;

namespace TicketShop.Service.Implementation {
    public class OrderService: IOrderService {
        private readonly IRepository<Order> _orderRepository;
        public OrderService(IRepository<Order> orderRepository) {
            this._orderRepository = orderRepository;

        }
        public List<Order> getAllOrders(TicketShopUser user) {
            var allOrders = this._orderRepository.GetAll().ToList();
            var orders = new List<Order>();

            foreach (var order in allOrders) {
                if (order.UserId == user.Id) {
                    orders.Add(order);
                }
            }

            return orders;
        }

        public Order getOrderDetails(Guid? id) {
            return this._orderRepository.Get(id);
        }
    }
}
