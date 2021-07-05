using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Repository.Interface;
using TicketShop.Service.Interface;

namespace TicketShop.Service.Implementation {
    public class ShoppingCartService: IShoppingCartService {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository, IUserRepository userRepository) {
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _userRepository = userRepository;

        }

        public bool DeleteProductFromShoppingCart(string userId, Guid id) {
            if (!string.IsNullOrEmpty(userId) && id != null) {
                var user = this._userRepository.Get(userId);

                var shoppingCart = user.UserCart;
                var itemToDelete = shoppingCart.TicketsInShoppingCart.Where(x => x.TicketId.Equals(id)).FirstOrDefault();

                shoppingCart.TicketsInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepository.Update(shoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCartDTO GetShoppingCartInfo(string userId) {
            var user = _userRepository.Get(userId);

            var shoppingCart = user.UserCart;

            var prices = shoppingCart.TicketsInShoppingCart
                .Select(x => new {
                    TicketPrice = x.Ticket.Price,
                    Quantity = x.Quantity
                })
                .ToList();

            var total = 0;

            foreach (var item in prices) {
                total += item.TicketPrice * item.Quantity;
            }

            ShoppingCartDTO shoppingCartDto = new ShoppingCartDTO {
                TicketsInShoppingCart = shoppingCart.TicketsInShoppingCart.ToList(),
                TotalPrice = total
            };

            return shoppingCartDto;
        }

        public bool OrderNow(string userId) {
            if (!string.IsNullOrEmpty(userId)) {
                var user = this._userRepository.Get(userId);

                var shoppingCart = user.UserCart;

                var order = new Order {
                    Id = Guid.NewGuid(),
                    User = user,
                    UserId = userId
                };

                _orderRepository.Insert(order);

                List<TicketInOrder> productInOrders = new List<TicketInOrder>();

                var result = shoppingCart.TicketsInShoppingCart.Select(z => new TicketInOrder {
                    Id = Guid.NewGuid(),
                    TicketId = z.Ticket.Id,
                    OrderedTicket = z.Ticket,
                    OrderId = order.Id,
                    UserOrder = order
                }).ToList();

                productInOrders.AddRange(result);

                foreach (var item in productInOrders) {
                    this._ticketInOrderRepository.Insert(item);
                }

                user.UserCart.TicketsInShoppingCart.Clear();

                this._userRepository.Update(user);

                return true;
            }

            return false;
        }
    }
}
