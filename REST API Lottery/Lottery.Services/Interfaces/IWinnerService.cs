using Lottery.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lottery.Services.Interfaces
{
    public interface IWinnerService
    {
        Task<IEnumerable<WinnerDTO>> GetAllWinnersAsync();
        Task<WinnerDTO> GetWinnerByIdAsync(Guid winnerId);
        Task CheckForWinnersAsync();
        Task<bool> DeleteWinnerAsync(Guid winnerId);
        Task<CheckModel> CheckNumbersAsync(string numbers);
    }
}
