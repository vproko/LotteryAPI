using System;

namespace Lottery.DataModels.Models
{
    public class DrawDTO
    {
        public Guid DrawId { get; set; }
        public DateTime Date { get; set; }
        public string DrawnNumbers { get; set; }
        public Guid SessionId { get; set; }
    }
}
