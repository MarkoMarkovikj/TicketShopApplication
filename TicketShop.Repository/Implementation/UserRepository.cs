using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.Identity;
using TicketShop.Repository.Interface;
using TicketShop.Web.Data;

namespace TicketShop.Repository.Implementation {
    public class UserRepository: IUserRepository {
        private readonly ApplicationDbContext context;
        private DbSet<TicketShopUser> entities;

        public UserRepository(ApplicationDbContext context) {
            this.context = context;
            this.entities = context.Set<TicketShopUser>();
        }
        public void Delete(TicketShopUser entity) {
            if (entities == null) {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public TicketShopUser Get(string id) {
            return this.entities
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.TicketsInShoppingCart)
                .Include("UserCart.TicketsInShoppingCart.Ticket")
                .SingleOrDefault(z => z.Id == id);
        }

        public IEnumerable<TicketShopUser> GetAll() {
            return entities.AsEnumerable();
        }

        public void Insert(TicketShopUser entity) {
            if (entities == null) {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(TicketShopUser entity) {
            if (entities == null) {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }
    }
}
