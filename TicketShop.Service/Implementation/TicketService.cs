using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Repository.Interface;
using TicketShop.Service.Interface;

namespace TicketShop.Service.Implementation {
    public class TicketService: ITicketService {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(IRepository<Ticket> ticketRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository, IUserRepository userRepository) {
            _ticketRepository = ticketRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
            _userRepository = userRepository;
        }

        public bool AddToShoppingCart(AddToShoppingCartDTO item, string userId) {
            var user = this._userRepository.Get(userId);
            var cart = user.UserCart;
            if(item.TicketId != null && cart != null) {
                var ticket = this._ticketRepository.Get(item.TicketId);

                if (ticket != null) {
                    TicketInShoppingCart itemToAdd = new TicketInShoppingCart {
                        Id = Guid.NewGuid(),
                        Ticket = ticket,
                        TicketId = ticket.Id,
                        ShoppingCart = cart,
                        ShoppingCartId = cart.Id,
                        Quantity = item.Quantity
                    };

                    this._ticketInShoppingCartRepository.Insert(itemToAdd);

                    return true;
                }
            }
            return false;
        }

        public void CreateNewTicket(Ticket ticket) {
            this._ticketRepository.Insert(ticket);
        }

        public void DeleteTicket(Guid id) {
            var ticket = this.GetDetailsForTicket(id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets() {
            return this._ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id) {
            return this._ticketRepository.Get(id);
        }

        public AddToShoppingCartDTO GetShoppingCartInfo(Guid? id) {
            var ticket = this._ticketRepository.Get(id);

            AddToShoppingCartDTO model = new AddToShoppingCartDTO {
                TicketId = ticket.Id,
                SelectedTicket = ticket,
                Quantity = 1
            };

            return model;
        }

        public void UpdateExistingTicket(Ticket ticket) {
            this._ticketRepository.Update(ticket);
        }
    }
}
