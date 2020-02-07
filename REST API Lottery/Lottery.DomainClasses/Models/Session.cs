using System;
using System.Collections.Generic;

namespace Lottery.DomainClasses.Models
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public Guid DrawId { get; set; }
        public virtual Draw Draw { get; set; }

        public virtual IEnumerable<Ticket> Tickets { get; set; }
        public virtual IEnumerable<Winner> Winners { get; set; }
    }
}
