using System;
using System.Collections.Generic;

namespace Lottery.DataModels.Models
{
    public class SessionDTO
    {
        public Guid SessionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid DrawId { get; set; }
        public IEnumerable<TicketDTO> Tickets { get; set; }
        public int? Winners { get; set; }
    }
}
