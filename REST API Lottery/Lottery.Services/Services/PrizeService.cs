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
    public class PrizeService : IPrizeService
    {
        private readonly IRepository<Prize> _prizeRepository;
        private readonly IMapper _mapper;

        public PrizeService(IRepository<Prize> prizeRepository, IMapper mapper)
        {
            _prizeRepository = prizeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrizeDTO>> GetAllPrizesAsync()
        {
            return await Task.Run(() => _mapper.Map<IEnumerable<PrizeDTO>>(_prizeRepository.GetAll().OrderByDescending(p => p.NumberOfHits)));
        }

        public async Task<PrizeDTO> GetPrizeByIdAsync(Guid prizeId)
        {
            return await Task.Run(() => _mapper.Map<PrizeDTO>(_prizeRepository.GetById(prizeId)));
        }

        public async Task<bool> CreatePrizeAsync(PrizeDTO prize)
        {
            bool check = await PrizeWithSameNumOfHitsAsync(prize.NumberOfHits);

            if (!check)
            {
                Prize newPrize = _mapper.Map<Prize>(prize);

                int result = await Task.Run(() => _prizeRepository.Add(newPrize));

                if (result != -1) return true; 
            }

            return false;
        }

        public async Task<bool> UpdatePrizeAsync(PrizeDTO prize)
        {
            Prize match = await Task.Run(() => _prizeRepository.GetById(prize.PrizeId));

            if(match != null)
            {
                match.Name = !String.IsNullOrEmpty(prize.Name) ? prize.Name : match.Name;
                match.NumberOfHits = prize.NumberOfHits > 0 ? prize.NumberOfHits : match.NumberOfHits;

                int result = await Task.Run(() => _prizeRepository.Update(match));

                if(result != -1) return true;
            }
            
            return false;
        }

        public async Task<bool> DeletePrizeAsync(Guid prizeId)
        {
            Prize match = await Task.Run(() => _prizeRepository.GetById(prizeId));

            if (match != null)
            {
                int result = await Task.Run(() => _prizeRepository.Delete(prizeId));

                if (result != -1) return true;
            }

            return false;
        }

        private async Task<bool> PrizeWithSameNumOfHitsAsync(int numOfHits)
        {
            Prize prize = await Task.Run(() => _prizeRepository.GetAll().FirstOrDefault(p => p.NumberOfHits == numOfHits));

            if (prize == null) return false;

            return true;
        }
    }
}
