using System;

namespace Lottery.DomainClasses.Models
{
    public class Ticket
    {
        public Guid TicketId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Numbers { get; set; }


        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }
    }
}
