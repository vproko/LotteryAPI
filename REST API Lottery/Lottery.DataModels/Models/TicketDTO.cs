using System;

namespace Lottery.DataModels.Models
{
    public class TicketDTO
    {
        public Guid TicketId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Numbers { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
    }
}
