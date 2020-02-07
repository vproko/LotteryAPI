using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.DataAccess.Repositories
{
    public class BaseRepository
    {
        protected readonly LotteryDbContext _context;

        public BaseRepository(LotteryDbContext context) => _context = context;


    }
}
