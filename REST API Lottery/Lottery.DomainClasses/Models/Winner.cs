using System;

namespace Lottery.DomainClasses.Models
{
    public class Winner
    {
        public Guid WinnerId { get; set; }
        public int NumberOfHits { get; set; }
        public string WinningNumbers { get; set; }
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }

        public Guid PrizeId { get; set; }
        public virtual Prize Prize { get; set; }
        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }
    }
}
