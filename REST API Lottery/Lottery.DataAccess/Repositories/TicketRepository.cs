using Lottery.DataAccess.Interfaces;
using Lottery.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.DataAccess.Repositories
{
    public class TicketRepository : BaseRepository, IRepository<Ticket>
    {
        public TicketRepository(LotteryDbContext context) : base(context) { }

        public IEnumerable<Ticket> GetAll()
        {
            return _context.Tickets;
        }

        public Ticket GetById(Guid id)
        {
            return _context.Tickets.Include(t => t.User).Include(t => t.Session).FirstOrDefault(t => t.TicketId == id);
        }

        public int Add(Ticket entity)
        {
            _context.Tickets.Add(entity);
            return _context.SaveChanges();
        }

        public int Update(Ticket entity)
        {
            _context.Tickets.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete(Guid id)
        {
            Ticket ticket = _context.Tickets.FirstOrDefault(t => t.TicketId == id);

            if(ticket != null)
            {
                _context.Tickets.Remove(ticket);
                return _context.SaveChanges();
            }

            return -1;
        }
    }
}
