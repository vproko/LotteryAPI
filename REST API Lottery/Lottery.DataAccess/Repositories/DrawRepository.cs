using Lottery.DomainClasses.Models;
using Lottery.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.DataAccess.Repositories
{
    public class DrawRepository : BaseRepository, IRepository<Draw>
    {
        public DrawRepository(LotteryDbContext context) : base(context) { }

        public IEnumerable<Draw> GetAll()
        {
            return _context.Drawings;
        }

        public Draw GetById(Guid id)
        {
            return _context.Drawings.FirstOrDefault(d => d.DrawId == id);
        }

        public int Add(Draw entity)
        {
            _context.Drawings.Add(entity);
            return _context.SaveChanges();
        }

        public int Update(Draw entity)
        {
            _context.Drawings.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete(Guid id)
        {
            Draw draw = _context.Drawings.FirstOrDefault(d => d.DrawId == id);

            if (draw != null)
            {
                _context.Drawings.Remove(draw);
                return _context.SaveChanges();
            }

            return -1;
        }
    }
}
