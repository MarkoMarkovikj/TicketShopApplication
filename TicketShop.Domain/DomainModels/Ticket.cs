using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain.DomainModels {
    public class Ticket: BaseEntity {
        public DateTime ValidTo { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public String MovieName { get; set; }
        public String MovieGenre { get; set; }
        public String Image { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketsInShoppingCart { get; set; }
        public IEnumerable<TicketInOrder> TicketsInOrder { get; set; }

    }
}
