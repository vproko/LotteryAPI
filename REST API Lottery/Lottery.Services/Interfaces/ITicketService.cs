using Lottery.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync();
        Task<TicketDTO> GetTicketByIdAsync(Guid ticketId);
        Task<bool> CreateTicketAsync(TicketDTO ticket);
        Task<bool> DeleteTicketAsync(Guid ticketId);
    }
}
