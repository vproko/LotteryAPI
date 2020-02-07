using AutoMapper;
using Lottery.DataAccess.Interfaces;
using Lottery.DataModels.Models;
using Lottery.DomainClasses.Models;
using Lottery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lottery.Services.Services
{
    public class SessionService : ISessionService
    {
        private readonly IRepository<Session> _sessionRepository;
        private readonly IMapper _mapper;

        public SessionService(IRepository<Session> sessionRepository, IMapper mapper)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionDTO>> GetAllSessions()
        {
            return  await Task.Run(() => _mapper.Map<IEnumerable<SessionDTO>>(_sessionRepository.GetAll()));
        }

        public async Task<SessionDTO> GetSessionById(Guid sessionId)
        {
            return await Task.Run(() => _mapper.Map<SessionDTO>(_sessionRepository.GetById(sessionId)));
        }

        public async Task<SessionDTO> CreateSessionAsync()
        {
            SessionDTO check = await IsThereActiveSessionAsync();

            if (check == null)
            {
                SessionDTO session = new SessionDTO()
                {
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(7),
                };

                Session newSession = _mapper.Map<Session>(session);
                int result = await Task.Run(() => _sessionRepository.Add(newSession));

                if(result != -1) return _mapper.Map<SessionDTO>(newSession);
            }

            return null;
        }

        public async Task<SessionDTO> IsThereActiveSessionAsync()
        {
            Session lastSession = await Task.Run(() => _sessionRepository.GetAll().OrderBy(s => s.StartDate).LastOrDefault());

            if(lastSession != null)
            {
                if(lastSession.StartDate.Date <= DateTime.UtcNow.Date && DateTime.UtcNow.Date <= lastSession.EndDate.Date)
                {
                    if(lastSession.DrawId == Guid.Empty)
                    {
                        return _mapper.Map<SessionDTO>(lastSession);
                    }
                }
            }

            return null;
        }

        public async Task<bool> DeleteSessionAsync(Guid sessionId)
        {
            Session match = await Task.Run(() => _sessionRepository.GetById(sessionId));

            if(match != null)
            {
                int result = await Task.Run(() => _sessionRepository.Delete(match.SessionId));

                if (result != -1) return true;
            }

            return false;
        }
    }
}
