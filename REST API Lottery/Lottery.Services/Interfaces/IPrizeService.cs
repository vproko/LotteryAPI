using Lottery.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lottery.Services.Interfaces
{
    public interface IPrizeService
    {
        Task<IEnumerable<PrizeDTO>> GetAllPrizesAsync();
        Task<PrizeDTO> GetPrizeByIdAsync(Guid prizeId);
        Task<bool> CreatePrizeAsync(PrizeDTO prize);
        Task<bool> UpdatePrizeAsync(PrizeDTO prize);
        Task<bool> DeletePrizeAsync(Guid prizeId);
    }
}
