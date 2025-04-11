using Blog.Application.Contracts.Authentication;
using Blog.Application.Interfaces.Authentication;
using Blog.Domain.Entities;
using Blog.Infrastructure.Authentication.Configurations;
using Blog.Infrastructure.Hasher;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        private readonly UserManager<User> _userManager;
        public JwtService(IOptions<JwtSettings> optionsJWTSettings, ITokenService tokenService
            , UserManager<User> userManager)
        {
            _jwtSettings = optionsJWTSettings.Value;
            this.tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<TokenResponse> GenerateToken(GenerateTokenRequest tokenRequest)
        {

            var accessToken = GenerateAccessToken(tokenRequest.Claims);

            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var hashedAccessToken = TokenHasher.HashToken(accessToken);
            var hashedRefreshToken = TokenHasher.HashToken(refreshToken);

            await tokenService.SaveRefreshTokenAsync(tokenRequest.UserId, hashedAccessToken, hashedRefreshToken, DateTime.UtcNow.AddDays(7));

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

            var user = await _userManager.FindByIdAsync(userToken.UserId.ToString());

            var claims = new List<Claim>
             {
               new Claim(ClaimTypes.Name, user.Name),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
             };
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var accessToken = GenerateAccessToken(claims);
            await tokenService.RevokeRefreshTokenAsync(hashedRefreshToken);

            var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var newHashedRefreshToken = TokenHasher.HashToken(newRefreshToken);
            var newHashedToken = TokenHasher.HashToken(accessToken);

            await tokenService.SaveRefreshTokenAsync(userToken.UserId, newHashedToken,
                newHashedRefreshToken, DateTime.UtcNow.AddDays(7));

            return new TokenResponse
            {
                Token = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            var hashedAccessToken = TokenHasher.HashToken(token);
            var userTokenIsValid = await tokenService.IsTokenValidAsync(hashedAccessToken);
            return userTokenIsValid;
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationMinutes),
                signingCredentials: creds
            );

            #region Logs
            var utcNow = DateTime.UtcNow;
            var expirationUtc = utcNow.AddSeconds(5);

            // تبدیل به ساعت محلی ایران
            var iranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
            var iranNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, iranTimeZone);
            var iranExpiration = TimeZoneInfo.ConvertTimeFromUtc(expirationUtc, iranTimeZone);

            // تبدیل به شمسی
            var pc = new PersianCalendar();
            string shamsiNow = $"{pc.GetYear(iranNow)}/{pc.GetMonth(iranNow):00}/{pc.GetDayOfMonth(iranNow):00} {iranNow:HH:mm:ss}";
            string shamsiExpiration = $"{pc.GetYear(iranExpiration)}/{pc.GetMonth(iranExpiration):00}/{pc.GetDayOfMonth(iranExpiration):00} {iranExpiration:HH:mm:ss}";

            // لاگ‌گیری دقیق
            Console.WriteLine("🕒 Token Time Details:");
            Console.WriteLine($"[UTC Now]            {utcNow:yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"[UTC Expiration]     {expirationUtc:yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"[Iran Now]           {iranNow:yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"[Iran Expiration]    {iranExpiration:yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"[Shamsi Now]         {shamsiNow}");
            Console.WriteLine($"[Shamsi Expiration]  {shamsiExpiration}");
            #endregion
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
