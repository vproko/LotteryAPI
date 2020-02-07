using Lottery.DataAccess.Interfaces;
using Lottery.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.DataAccess.Repositories
{
    public class SessionRepository : BaseRepository, IRepository<Session>
    {
        public SessionRepository(LotteryDbContext context) : base(context) { }

        public IEnumerable<Session> GetAll()
        {
            return _context.Sessions.Include(s => s.Tickets);
        }

        public Session GetById(Guid id)
        {
            return _context.Sessions.Include(s => s.Tickets).Include(s => s.Winners).FirstOrDefault(s => s.SessionId == id);
        }

        public int Add(Session entity)
        {
            _context.Sessions.Add(entity);
            return _context.SaveChanges();
        }

        public int Update(Session entity)
        {
            _context.Sessions.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete(Guid id)
        {
            Session session = _context.Sessions.FirstOrDefault(s => s.SessionId == id);

            if(session != null)
            {
                _context.Sessions.Remove(session);
                return _context.SaveChanges();
            }

            return -1;
        }
    }
}
