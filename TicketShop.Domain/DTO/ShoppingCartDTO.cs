using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.DTO {
    public class ShoppingCartDTO {
        public List<TicketInShoppingCart> TicketsInShoppingCart { get; set; }
        public int TotalPrice { get; set; }
    }
}
