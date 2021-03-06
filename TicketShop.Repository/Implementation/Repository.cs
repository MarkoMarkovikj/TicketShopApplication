using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Repository.Interface;
using TicketShop.Web.Data;

namespace TicketShop.Repository.Implementation {
    public class Repository<T>: IRepository<T> where T : BaseEntity {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context) {
            this.context = context;
            this.entities = context.Set<T>();
        }
        public void Delete(T entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            entities.Remove(entity);
            context.SaveChanges();
        }

        public T Get(Guid? id) {
            return entities.SingleOrDefault(z => z.Id == id);
        }

        public IEnumerable<T> GetAll() {
            return entities.AsEnumerable();
        }

        public List<T> GetAllByGenre(string genre) {
            return entities
                .Select(z => z)
                .Where(z => (z as Ticket).MovieGenre == genre)
                .ToList();
        }

        public void Insert(T entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            entities.Update(entity);
            context.SaveChanges();
        }
    }
}
