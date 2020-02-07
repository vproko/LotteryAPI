using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.DataModels.Models
{
    public class PrizeDTO
    {
        public Guid PrizeId { get; set; }
        public string Name { get; set; }
        public int NumberOfHits { get; set; }
    }
}
