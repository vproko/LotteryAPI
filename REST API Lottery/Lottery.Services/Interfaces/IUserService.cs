using Lottery.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> AuthenticateAsync(LoginModel login);
        Task<bool> UserRegistrationAsync(RegisterModel model, string role);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUSerByIdAsync(Guid userId);
        Task<UserDTO> UpdateUserAsync(UpdateModel user);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
