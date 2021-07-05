using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.DTO {
    public class AddToShoppingCartDTO {
        public Guid TicketId { get; set; }
        public Ticket SelectedTicket { get; set; }
        public int Quantity { get; set; }
    }
}
