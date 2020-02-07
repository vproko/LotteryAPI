using Lottery.DataAccess.Interfaces;
using Lottery.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.DataAccess.Repositories
{
    public class UserRepository : BaseRepository, IRepository<User>
    {
        public UserRepository(LotteryDbContext context) : base(context) { }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(u => u.Tickets);
        }

        public User GetById(Guid id)
        {
            return _context.Users.Include(u => u.Tickets).FirstOrDefault(u => u.UserId == id);

        }

        public int Add(User entity)
        {
            _context.Users.Add(entity);
            return _context.SaveChanges();
        }

        public int Update(User entity)
        {
            _context.Users.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete(Guid id)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if(user != null)
            {
                _context.Users.Remove(user);
                return _context.SaveChanges();
            }

            return -1;
        }
    }
}
