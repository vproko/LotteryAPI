using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.DomainClasses.Models
{
    public class Prize
    {
        public Guid PrizeId { get; set; }
        public string Name { get; set; }
        public int NumberOfHits { get; set; }

        public virtual IEnumerable<Winner> Winners { get; set; }
    }
}
