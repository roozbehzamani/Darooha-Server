using Darooha.Common.Helpers.AppSetting;
using Darooha.Common.Helpers.Interface;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Common.Token;
using Darooha.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Common.Helpers.Helpers
{
    public class Utilities : IUtilities
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Tbl_User> _userManager;
        private readonly DaroohaDbContext _db;
        private readonly IHttpContextAccessor _http;
        private readonly TokenSetting _tokenSetting;
        public Utilities(DaroohaDbContext dbContext, UserManager<Tbl_User> userManager, IHttpContextAccessor http, IConfiguration config)
        {
            _config = config;
            _db = dbContext;
            _userManager = userManager;
            _http = http;
            var tokenSettingSection = _config.GetSection("TokenSetting");
            _tokenSetting = tokenSettingSection.Get<TokenSetting>();
        }

        #region token
        public async Task<TokenResponseDTO> GenerateNewTokenAsync(TokenRequestDTO TokenRequestDTO)
        {
            var allUser = await _userManager.FindByIdAsync("6928c4c5-657b-43d3-8188-8ef2a4c9da6b");
            var user = await _userManager.FindByNameAsync(TokenRequestDTO.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, TokenRequestDTO.Password))
            {
                //create new token
                var newRefreshToken = CreateRefreshToken(_tokenSetting.ClientId, user.Id, TokenRequestDTO.IsRemember);
                //remove older tokens
                var oldRefreshToken = await _db.Tokens.Where(p => p.UserId == user.Id).ToListAsync();

                if (oldRefreshToken.Any())
                {
                    foreach (var ort in oldRefreshToken)
                    {
                        _db.Tokens.Remove(ort);
                    }
                }
                //add new refresh token to db
                _db.Tokens.Add(newRefreshToken);

                await _db.SaveChangesAsync();

                var accessToken = await CreateAccessTokenAsync(user, newRefreshToken.Value);

                return new TokenResponseDTO()
                {
                    token = accessToken.token,
                    refresh_token = accessToken.refresh_token,
                    status = true,
                    user = user
                };
            }
            else
            {
                return new TokenResponseDTO()
                {
                    status = false,
                    message = "کاربری با این یوزر و پس وجود ندارد"
                };
            }
        }
        public async Task<TokenResponseDTO> CreateAccessTokenAsync(Tbl_User user, string refreshToken)
        {
            double tokenExpireTime = Convert.ToDouble(_tokenSetting.ExpireTime);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenSetting.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _tokenSetting.Site,
                Audience = _tokenSetting.Audience,
                Expires = DateTime.Now.AddMinutes(tokenExpireTime),
                SigningCredentials = creds
            };

            var newAccessToken = tokenHandler.CreateToken(tokenDes);

            var encodedAccessToken = tokenHandler.WriteToken(newAccessToken);

            return new TokenResponseDTO()
            {
                token = encodedAccessToken,
                refresh_token = refreshToken
            };
        }
        public Token CreateRefreshToken(string clientId, string userId, bool isRemember)
        {
            return new Token()
            {
                ClientId = clientId,
                UserId = userId,
                Value = Guid.NewGuid().ToString("N"),
                ExpireTime = isRemember ? DateTime.Now.AddDays(7) : DateTime.Now.AddDays(1),
                Ip = _http.HttpContext.Connection != null ?
                    _http.HttpContext.Connection.RemoteIpAddress != null ?
                    _http.HttpContext.Connection.RemoteIpAddress.ToString() :
                    "noIp" :
                    "noIp"
            };
        }
        #endregion

        #region tokenRefresh

        public async Task<TokenResponseDTO> RefreshAccessTokenAsync(TokenRequestDTO TokenRequestDTO)
        {
            string ip = _http.HttpContext.Connection != null
                    ? _http.HttpContext.Connection.RemoteIpAddress != null
                        ?
                        _http.HttpContext.Connection.RemoteIpAddress.ToString()
                        :
                        "noIp"
                    : "noIp";


            var refreshToken = await _db.Tokens.FirstOrDefaultAsync(p =>
                 p.ClientId == _tokenSetting.ClientId && p.Value == TokenRequestDTO.RefreshToken
                 && p.Ip == ip);

            if (refreshToken == null)
            {
                return new TokenResponseDTO()
                {
                    status = false,
                    message = "خطا در اعتبار سنجی خودکار"
                };
            }
            if (refreshToken.ExpireTime < DateTime.Now)
            {
                return new TokenResponseDTO()
                {
                    status = false,
                    message = "خطا در اعتبار سنجی خودکار"
                };
            }

            var user = await _userManager.FindByIdAsync(refreshToken.UserId);

            if (user == null)
            {
                return new TokenResponseDTO()
                {
                    status = false,
                    message = "خطا در اعتبار سنجی خودکار"
                };
            }

            var response = await CreateAccessTokenAsync(user, refreshToken.Value);

            return new TokenResponseDTO()
            {
                status = true,
                token = response.token
            };
        }

        #endregion

        #region password
        public void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
            }
        }

        public bool VerifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != PasswordHash[i])
                        return false;
                }
            }
            return true;
        }
        #endregion
    }
}