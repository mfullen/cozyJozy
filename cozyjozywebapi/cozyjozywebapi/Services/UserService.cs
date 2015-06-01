using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Services
{
    public interface IUserService
    {
        string GeneratePasswordResetToken(string userId);

        IdentityResult ResetPassword(string userId, string code, string password);
    }

    public class UserService : IUserService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly UserManager<User> _userManager;
        public UserService(ITokenRepository tokenRepository, UserManager<User> userManager)
        {
            _tokenRepository = tokenRepository;
            _userManager = userManager;
        }

        public string GeneratePasswordResetToken(string userId)
        {
           var token = _tokenRepository.Add(new Token()
            {
                TokenCode = Guid.NewGuid().ToString(),
                TokenType = TokenType.PasswordReset,
                UserId = userId
            });
            return token.TokenCode;
        }

        public IdentityResult ResetPassword(string userId, string code, string password)
        {
            var token = _tokenRepository.Find(userId, code);
            if (token == null)
            {
                return new IdentityResult(new List<string>
                {
                    "Code is not valid"
                });
            }
            var removePw = _userManager.RemovePassword(userId);
            if (!removePw.Succeeded)
                return removePw;
            var addPw = _userManager.AddPassword(userId, password);
            _tokenRepository.Delete(token);
            return addPw;
        }
    }
}
