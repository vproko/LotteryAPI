using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.DomainClasses.Models
{
    public class Draw
    {
        public Guid DrawId { get; set; }
        public DateTime Date { get; set; }
        public string DrawnNumbers { get; set; }


        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }
    }
}
