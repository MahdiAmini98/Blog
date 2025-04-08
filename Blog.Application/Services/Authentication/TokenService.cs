using Blog.Application.Interfaces.Authentication;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services.Authentication
{
    public class TokenService : ITokenService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserToken> _userTokenRepository;

        public TokenService(IUnitOfWork unitOfWork, IRepository<UserToken> userTokenRepository)
        {
            _unitOfWork = unitOfWork;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<UserToken?> GetRefreshTokenAsync(string hashedRefreshToken)
        {
            var tokens = await _unitOfWork.Repository<UserToken>()
                .FindAsync(t =>
                   t.RefreshToken == hashedRefreshToken &&
                   !t.IsRevoked &&
                   t.ExpirationTime > DateTime.UtcNow);

            return tokens.FirstOrDefault();
        }

        public async Task<bool> IsTokenValidAsync(string hashedAccessToken)
        {
            var usertokens = await _userTokenRepository.FindAsync(t => t.AccessToken == hashedAccessToken);

            var userToken = usertokens.FirstOrDefault();

            if (userToken == null || !userToken.IsValid())
            {
                return false;//توکن معتبر نیست
            }
            return true;//توکن معتبر است

        }

        public async Task RevokeRefreshTokenAsync(string hashedRefreshToken)
        {
            var userToken = await GetRefreshTokenAsync(hashedRefreshToken);
            if (userToken == null)
                throw new KeyNotFoundException("Refresh token not found.");

            userToken.Revoke();
            await _unitOfWork.CommitAsync();
        }

        public async Task SaveRefreshTokenAsync(Guid userId, string hashedAccessToken, string hashedRefreshToken, DateTime expirationTime)
        {
            var userToken = new UserToken(userId, hashedAccessToken, hashedRefreshToken, expirationTime);
            _userTokenRepository.Add(userToken);
            await _unitOfWork.CommitAsync();
        }
    }
}
