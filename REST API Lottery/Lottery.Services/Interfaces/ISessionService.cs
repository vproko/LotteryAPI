using Lottery.DataModels.Models;
using Lottery.DomainClasses.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services.Interfaces
{
    public interface ISessionService
    {
        Task<SessionDTO> CreateSessionAsync();
        Task<IEnumerable<SessionDTO>> GetAllSessions();
        Task<SessionDTO> GetSessionById(Guid sessionId);
        Task<SessionDTO> IsThereActiveSessionAsync();
        Task<bool> DeleteSessionAsync(Guid sessionId);
    }
}
