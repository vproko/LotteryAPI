using Lottery.DataAccess.Interfaces;
using Lottery.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.DataAccess.Repositories
{
    public class WinnerRepository : BaseRepository, IRepository<Winner>
    {
        public WinnerRepository(LotteryDbContext context) : base(context) { }

        public IEnumerable<Winner> GetAll()
        {
            return _context.Winners;
        }

        public Winner GetById(Guid id)
        {
            return _context.Winners.FirstOrDefault(w => w.WinnerId == id);
        }

        public int Add(Winner entity)
        {
            _context.Winners.Add(entity);
            return _context.SaveChanges();
        }

        public int Update(Winner entity)
        {
            _context.Winners.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete(Guid id)
        {
            Winner winner = _context.Winners.FirstOrDefault(w => w.WinnerId == id);

            if(winner != null)
            {
                _context.Winners.Remove(winner);
                return _context.SaveChanges();
            }

            return -1;
        }
    }
}
