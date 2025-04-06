using Blog.Application.Contracts.Authentication;
using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.Authentication
{
    public interface ITokenService
    {
        Task SaveRefreshTokenAsync(Guid userId, string hashedAccessToken, string hashedRefreshToken, DateTime expirationTime);
        Task<UserToken?> GetRefreshTokenAsync(string hashedRefreshToken);
        Task RevokeRefreshTokenAsync(string hashedRefreshToken);
    }
}
