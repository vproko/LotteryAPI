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
    public class WinnerService : IWinnerService
    {
        private readonly IRepository<Winner> _winnerRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<Draw> _drawRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Prize> _prizeRepository;
        private readonly IMapper _mapper;

        public WinnerService(IRepository<Winner> winnerRepository, 
                             IRepository<Session> sessionRepository, 
                             IRepository<Draw> drawRepository, 
                             IRepository<Ticket> ticketRepository,
                             IRepository<Prize> prizeRepository,
                             IMapper mapper)
        {
            _winnerRepository = winnerRepository;
            _sessionRepository = sessionRepository;
            _drawRepository = drawRepository;
            _ticketRepository = ticketRepository;
            _prizeRepository = prizeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WinnerDTO>> GetAllWinnersAsync()
        {
            return await Task.Run(() => _mapper.Map<IEnumerable<WinnerDTO>>(_winnerRepository.GetAll()));
        }

        public async Task<WinnerDTO> GetWinnerByIdAsync(Guid winnerId)
        {
            IEnumerable<WinnerDTO> list = await Task.Run(() => _mapper.Map<IEnumerable<WinnerDTO>>(_winnerRepository.GetAll().Where(w => w.UserId == winnerId).OrderByDescending(w => w.NumberOfHits)));

            return list.FirstOrDefault(w => w.UserId == winnerId);
        }

        public async Task CheckForWinnersAsync()
        {
            Draw lastDraw = await Task.Run(() => _drawRepository.GetAll().OrderBy(d => d.Date).LastOrDefault());
            IEnumerable<Ticket> lastSessionTickets = await Task.Run(() => _ticketRepository.GetAll().Where(t => t.SessionId == lastDraw.SessionId));

            if(lastDraw != null && lastSessionTickets != null) 
            {
                await CheckTicketsAsync(lastSessionTickets, lastDraw);
            }
        }

        public async Task<bool> DeleteWinnerAsync(Guid winnerId)
        {
            Winner winner = await Task.Run(() => _winnerRepository.GetById(winnerId));

            if(winner != null)
            {
                int result = await Task.Run(() => _winnerRepository.Delete(winnerId));

                if (result != -1) return true;
            }

            return false;
        }

        public async Task<CheckModel> CheckNumbersAsync(string numbers)
        {
            Draw lastDraw = await Task.Run(() => _drawRepository.GetAll().OrderBy(d => d.Date).LastOrDefault());
            IEnumerable<Winner> winners = await Task.Run(() => _winnerRepository.GetAll().Where(w => w.SessionId == lastDraw.SessionId));
            
            if(winners != null)
            {
                IEnumerable<Ticket> tickets = await WinningTicketsASync(winners);

                if(tickets != null)
                {
                    Ticket check = await Task.Run(() => tickets.FirstOrDefault(t => t.Numbers == numbers));

                    if(check != null)
                    {
                        CheckModel result = await CheckThePrizeAsync(check);

                        return result;
                    }
                }
            }

            return null;
        }

        private async Task<CheckModel> CheckThePrizeAsync(Ticket ticket)
        {
            Winner winner = await Task.Run(() => _winnerRepository.GetAll().FirstOrDefault(w => w.TicketId == ticket.TicketId));
            Prize prize = await Task.Run(() => _prizeRepository.GetById(winner.PrizeId));
            CheckModel result = new CheckModel { Prize = prize.Name, WinningNumbers = winner.WinningNumbers };

            return result;
        }

        private async Task<IEnumerable<Ticket>> WinningTicketsASync(IEnumerable<Winner> winners)
        {
            List<Ticket> matchings = new List<Ticket>();

            foreach (Winner winner in winners)
            {
                Ticket match = await Task.Run(() => _ticketRepository.GetById(winner.TicketId));

                if (match != null) matchings.Add(match);
            }

            return matchings.AsEnumerable();
        }

        private async Task CheckTicketsAsync(IEnumerable<Ticket> lastSessionTickets, Draw lastDraw)
        {
            foreach (Ticket ticket in lastSessionTickets)
            {
                IEnumerable<int> ticketNumbers = ticket.Numbers.Split(',').Select(Int32.Parse).ToList().OrderBy(num => num);
                IEnumerable<int> drawnNumbers = lastDraw.DrawnNumbers.Split(',').Select(Int32.Parse).ToList().OrderBy(num => num);

                IEnumerable<int> matchings = ticketNumbers.Intersect(drawnNumbers).OrderBy(num => num);
                int numberOfHits = matchings.Count();

                if (numberOfHits >= 3)
                {
                    await CreateWinnerAsync(numberOfHits, ticket, matchings, lastDraw);
                }

                await RemoveDoubleWinnersAsync(lastDraw.SessionId);
            }
        }

        private async Task CreateWinnerAsync(int numberOfHits, Ticket ticket, IEnumerable<int> matchings, Draw lastDraw)
        {
            Prize prize = await Task.Run(() => _prizeRepository.GetAll().FirstOrDefault(p => p.NumberOfHits == numberOfHits));

            WinnerDTO winner = new WinnerDTO
            {
                NumberOfHits = numberOfHits,
                PrizeId = prize.PrizeId,
                WinningNumbers = String.Join(", ", matchings),
                UserId = ticket.UserId,
                TicketId = ticket.TicketId,
                SessionId = lastDraw.SessionId
            };

            await Task.Run(() => _winnerRepository.Add(_mapper.Map<Winner>(winner)));
        }

        private async Task RemoveDoubleWinnersAsync(Guid lastSessionId)
        {
            IEnumerable<WinnerDTO> winners = await Task.Run(() => _mapper.Map<IEnumerable<WinnerDTO>>(_winnerRepository.GetAll().Where(t => t.SessionId == lastSessionId)));
            foreach (var winner in winners)
            {
                var current = winner;
                var matches = winners.Where(x => x.UserId == current.UserId && x.TicketId != current.TicketId && x.NumberOfHits <= current.NumberOfHits);
                if (matches != null)
                {
                    foreach (var match in matches)
                    {
                        await Task.Run(() => _winnerRepository.Delete(match.WinnerId));
                    }
                }
            }
        }
    }
}
