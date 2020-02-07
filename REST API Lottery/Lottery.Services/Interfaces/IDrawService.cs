using Lottery.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lottery.Services.Interfaces
{
    public interface IDrawService
    {
        Task<IEnumerable<DrawDTO>> GetAllDrawsAsync();
        Task<DrawDTO> GetDrawByIdAsync(Guid drawId);
        Task<bool> CreateDrawAsync(DrawDTO drawing);
        Task<bool> DeleteDrawAsync(Guid drawId);
    }
}
