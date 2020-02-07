using System;

namespace Lottery.DataModels.Models
{
    public class WinnerDTO
    {
        public Guid WinnerId { get; set; }
        public int NumberOfHits { get; set; }
        public Guid PrizeId { get; set; }
        public string WinningNumbers { get; set; }
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
        public Guid SessionId { get; set; }
    }
}
