using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;

namespace TicketShop.Service.Interface {
    public interface ITicketService {
            List<Ticket> GetAllTickets();
            Ticket GetDetailsForTicket(Guid? id);
            void CreateNewTicket(Ticket ticket);
            void UpdateExistingTicket(Ticket ticket);
            AddToShoppingCartDTO GetShoppingCartInfo(Guid? id);
            void DeleteTicket(Guid id);
            bool AddToShoppingCart(AddToShoppingCartDTO item, string userId);
            List<Ticket> GetAllTicketsByGenre(string movieGenre);
    }
}
