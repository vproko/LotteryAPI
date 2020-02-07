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
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Draw> _drawRepository;
        private readonly IMapper _mapper;

        public TicketService(IRepository<Ticket> ticketRepository, 
                             IRepository<Session> sessionRepository, 
                             IRepository<User> userRepository, 
                             IRepository<Draw> drawRepository, 
                             IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _drawRepository = drawRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync()
        {
            return await Task.Run(() => _mapper.Map<IEnumerable<TicketDTO>>(_ticketRepository.GetAll()));
        }

        public async Task<TicketDTO> GetTicketByIdAsync(Guid ticketId)
        {
            return await Task.Run(() => _mapper.Map<TicketDTO>(_ticketRepository.GetById(ticketId)));
        }

        public async Task<bool> CreateTicketAsync(TicketDTO ticket)
        {
            Session activeSession = await Task.Run(() => ActiveSessionAsync());
            User isItValidUser = await Task.Run(() => _userRepository.GetAll().FirstOrDefault(u => u.UserId == ticket.UserId));

            if (activeSession != null && isItValidUser != null)
            {
                bool check = await CheckForDuplicateNumbersAsync(ticket);

                if (check)
                {
                    ticket.SessionId = activeSession.SessionId;
                    int result = await Task.Run(() => _ticketRepository.Add(_mapper.Map<Ticket>(ticket)));

                    if (result != -1) return true;

                }
            }

            return false;
        }

        public async Task<bool> DeleteTicketAsync(Guid ticketId)
        {
            Ticket match = await Task.Run(() => _ticketRepository.GetById(ticketId));

            if (match != null)
            {
                int result = await Task.Run(() => _ticketRepository.Delete(ticketId));

                if (result != -1) return true;

                return false;
            }

            return false;
        }

        private async Task<bool> CheckForDuplicateNumbersAsync(TicketDTO ticket)
        {
            var numbers = await Task.Run(() => ticket.Numbers.Split(',').Select(Int32.Parse).ToList());

            if (numbers.Count != numbers.Distinct().Count())
            {
                return false;
            }

            return true;
        }

        private async Task<Session> ActiveSessionAsync()
        {
            Session lastSession = await Task.Run(() => _sessionRepository.GetAll().OrderBy(s => s.StartDate).LastOrDefault());

            if (lastSession != null)
            {
                if (lastSession.StartDate.Date <= DateTime.UtcNow.Date && DateTime.UtcNow.Date <= lastSession.EndDate.Date)
                {
                    if(lastSession.DrawId == Guid.Empty)
                    {
                        return lastSession;
                    }
                }
            }

            return null;
        }
    }
}
