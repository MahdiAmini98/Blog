using Blog.Application.Contracts.Authentication;
using Blog.Application.Interfaces.Authentication;
using Blog.Infrastructure.Authentication.Configurations;
using Blog.Infrastructure.Hasher;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Authentication.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService tokenService;
        public JwtService(IOptions<JwtSettings> optionsJWTSettings, ITokenService tokenService)
        {
            _jwtSettings = optionsJWTSettings.Value;
            this.tokenService = tokenService;

        }

        public TokenResponse GenerateToken(GenerateTokenRequest tokenRequest)
        {

            var accessToken = GenerateAccessToken(tokenRequest.Claims);

            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var hashedAccessToken = TokenHasher.HashToken(accessToken);
            var hashedRefreshToken = TokenHasher.HashToken(refreshToken);

            tokenService.SaveRefreshTokenAsync(tokenRequest.UserId, hashedAccessToken, hashedRefreshToken, DateTime.UtcNow.AddDays(7));

            return new TokenResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
            };
        }


        public async Task<TokenResponse> RenewAccessTokenAsync(string refreshToken)
        {

            var hashedRefreshToken = TokenHasher.HashToken(refreshToken);
            var userToken = await tokenService.GetRefreshTokenAsync(hashedRefreshToken);

            if (userToken == null)
                throw new SecurityTokenException("Invalid or expired refresh token.");

            //تکمیل شود
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "testUser"),
            };

            var accessToken = GenerateAccessToken(claims);
            await tokenService.RevokeRefreshTokenAsync(hashedRefreshToken);

            var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var newHashedRefreshToken = TokenHasher.HashToken(newRefreshToken);

            await tokenService.SaveRefreshTokenAsync(userToken.UserId, newRefreshToken,
                newHashedRefreshToken, DateTime.UtcNow.AddDays(7));


            return new TokenResponse
            {
                Token = accessToken,
                RefreshToken = newRefreshToken
            };



        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireTime),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
