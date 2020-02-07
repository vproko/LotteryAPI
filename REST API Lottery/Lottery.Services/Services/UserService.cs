using AutoMapper;
using Lottery.DataAccess.Interfaces;
using Lottery.DataModels.Models;
using Lottery.DomainClasses.Models;
using Lottery.Services.Helpers;
using Lottery.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IOptions<AppSettings> options, IMapper mapper)
        {
            _userRepository = userRepository;
            _options = options;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            return await Task.Run(() => _mapper.Map<IEnumerable<UserDTO>>(_userRepository.GetAll().Where(u => u.Role == "User")));
        }

        public async Task<UserDTO> GetUSerByIdAsync(Guid userId)
        {
            UserDTO user = await Task.Run(() => _mapper.Map<UserDTO>(_userRepository.GetById(userId)));

            if(user != null) return user;

            throw new Exception("There's no user with that id.");
        }

        public async Task<bool> UserRegistrationAsync(RegisterModel model, string role)
        {
            model.Role = role;

            bool checkEmptyProperties = await Task.Run(() => CheckForEmptyProperies(model));

            if (checkEmptyProperties)
            {
                bool checkForExistingUserOrMail = await CheckForExistingMailOrUserAsync(model);

                if (checkForExistingUserOrMail)
                {
                    if (model.Password == model.ConfirmPassword)
                    {
                        var hashedPassword = await Task.Run(() => HashPassword(model.Password));
                        User user = _mapper.Map<User>(model);
                        user.Password = hashedPassword;

                        int result = await Task.Run(() => _userRepository.Add(user));

                        if (result != -1) return true;
                    }
                }
            }

            return false;
        }

        public async Task<UserDTO> AuthenticateAsync(LoginModel login)
        {
            var hashedPassword = await Task.Run(() => HashPassword(login.Password));

            UserDTO user = await Task.Run(() => _mapper.Map<UserDTO>(_userRepository.GetAll().SingleOrDefault(u => u.Username == login.Username && u.Password == hashedPassword)));

            if (user == null) return null;

            UserDTO authUser = await Task.Run(() => CreateToken(user));

            return authUser;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            User user = await Task.Run(() => _userRepository.GetById(userId));

            if (user != null)
            {
                int result = await Task.Run(() => _userRepository.Delete(userId));

                if (result != -1) return true;
            }

            return false;
        }

        public async Task<UserDTO> UpdateUserAsync(UpdateModel user)
        {
            User match = await Task.Run(() => _userRepository.GetById(user.UserId));

            if(match != null)
            {
                User infoUpdateCheck = await Task.Run(() => CheckForInfoChanges(match, user));

                if (!String.IsNullOrEmpty(user.OldPassword) && !String.IsNullOrEmpty(user.NewPassword) && !String.IsNullOrEmpty(user.ConfirmedPassword))
                {

                    string passUpdateCheck = await CheckForPasswordChangesAsync(infoUpdateCheck, user);

                    if (String.IsNullOrEmpty(passUpdateCheck)) return null;

                    infoUpdateCheck.Password = passUpdateCheck;
                }

                int result = await Task.Run(() => _userRepository.Update(infoUpdateCheck));

                infoUpdateCheck.Token = user.Token;

                if (result != -1) return _mapper.Map<UserDTO>(infoUpdateCheck);
            }

            return null;
        }

        private User CheckForInfoChanges(User match, UpdateModel user)
        {
            match.UserId = user.UserId;
            match.FirstName = !String.IsNullOrEmpty(user.FirstName) ? user.FirstName : match.FirstName;
            match.LastName = !String.IsNullOrEmpty(user.LastName) ? user.LastName : match.LastName;
            match.Email = !String.IsNullOrEmpty(user.Email) ? user.Email : match.Email;

            return match;
        }

        private async Task<string> CheckForPasswordChangesAsync(User match, UpdateModel user)
        {
            string oldPasswordHashed = await Task.Run(() => HashPassword(user.OldPassword));

            if (match.Password != oldPasswordHashed || user.NewPassword != user.ConfirmedPassword) return null;

            var newHashedPassword = await Task.Run(() => HashPassword(user.NewPassword));

            return newHashedPassword;
        }

        private UserDTO CreateToken(UserDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        private bool CheckForEmptyProperies(RegisterModel model)
        {
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                if (String.IsNullOrEmpty((string)property.GetValue(model, null)))
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> CheckForExistingMailOrUserAsync(RegisterModel model)
        {
            User usernameCheck = await Task.Run(() => _userRepository.GetAll().FirstOrDefault(u => u.Username.ToLower() == model.Username.ToLower()));
            User mailCheck = await Task.Run(() => _userRepository.GetAll().FirstOrDefault(u => u.Email == model.Email));

            if (usernameCheck == null && mailCheck == null)
            {
                return true;
            }

            return false;
        }

        private string HashPassword (string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            return hashedPassword;
        }
    }
}
