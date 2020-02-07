using Lottery.DomainClasses.Models;
using Lottery.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.DataAccess.Repositories
{
    public class PrizeRepository : BaseRepository, IRepository<Prize>
    {
        public PrizeRepository(LotteryDbContext context) : base(context) { }

        public IEnumerable<Prize> GetAll()
        {
            return _context.Prizes;
        }

        public Prize GetById(Guid id)
        {
            return _context.Prizes.FirstOrDefault(p => p.PrizeId == id);
        }

        public int Add(Prize entity)
        {
            _context.Prizes.Add(entity);
            return _context.SaveChanges();
        }

        public int Update(Prize entity)
        {
            _context.Prizes.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete(Guid id)
        {
            Prize prize = _context.Prizes.FirstOrDefault(p => p.PrizeId == id);

            if (prize != null)
            {
                _context.Prizes.Remove(prize);
                return _context.SaveChanges();
            }

            return -1;
        }
    }
}
