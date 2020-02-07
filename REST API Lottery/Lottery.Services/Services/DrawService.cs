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
    public class DrawService : IDrawService
    {
        private readonly IRepository<Draw> _drawRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IWinnerService _winnerService;
        private readonly IMapper _mapper;

        public DrawService(IRepository<Draw> drawRepository, 
                           IRepository<Session> sessionRepository, 
                           IWinnerService winnerService, 
                           IMapper mapper)
        {
            _drawRepository = drawRepository;
            _sessionRepository = sessionRepository;
            _winnerService = winnerService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DrawDTO>> GetAllDrawsAsync()
        {
            return await Task.Run(() => _mapper.Map<IEnumerable<DrawDTO>>(_drawRepository.GetAll()));
        }

        public async Task<DrawDTO> GetDrawByIdAsync(Guid drawId)
        {
            return await Task.Run(() => _mapper.Map<DrawDTO>(_drawRepository.GetById(drawId)));
        }

        public async Task<bool> CreateDrawAsync(DrawDTO drawing)
        {
            Session activeSession = await ActiveSessionAsync();

            if (activeSession != null)
            {
                bool check = await CheckForDuplicateNumbersAsync(drawing);

                if (check == true)
                {
                    bool result = await RegisterDrawCheckForWinnersAsync(drawing, activeSession);

                    if (result) return true;
                }
            }

            return false;
        }

        public async Task<bool> DeleteDrawAsync(Guid drawId)
        {
            Draw draw = await Task.Run(() => _drawRepository.GetById(drawId));

            if (draw != null)
            {
                int result = await Task.Run(() => _drawRepository.Delete(drawId));

                if(result != -1) return true;
            }
            
            return false;
        }

        private Session CheckDateRange(Session session)
        {
            if(session.StartDate.Date <= DateTime.UtcNow.Date && DateTime.UtcNow.Date <= session.EndDate.Date)
            {
                return session;
            }

            return null;
        }

        private async Task<Session> ActiveSessionAsync()
        {
            Session lastSession = await Task.Run(() => _sessionRepository.GetAll().OrderBy(s => s.StartDate).LastOrDefault());
            Session activeSession = await Task.Run(() => CheckDateRange(lastSession));

            if (activeSession != null && lastSession.DrawId == Guid.Empty) return activeSession;

            return null;
        }

        private async Task<bool> CheckForDuplicateNumbersAsync(DrawDTO drawing)
        {
            var numbers = await Task.Run(() => drawing.DrawnNumbers.Split(',').Select(Int32.Parse).ToList());

            if (numbers.Count != numbers.Distinct().Count()) return false;

            return true;
        }

        private async Task<bool> RegisterDrawCheckForWinnersAsync(DrawDTO drawing, Session activeSession)
        {
            drawing.SessionId = activeSession.SessionId;
            Draw draw = _mapper.Map<Draw>(drawing);
            activeSession.DrawId = draw.DrawId;

            int result1 = await Task.Run(() => _sessionRepository.Update(activeSession));
            int result2 = await Task.Run(() => _drawRepository.Add(draw));

            if (result1 != -1 && result2 != -1)
            {
                await _winnerService.CheckForWinnersAsync();

                return true;
            }

            return false;
        }
    }
}
