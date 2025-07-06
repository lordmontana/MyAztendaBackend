using AuthService.Models;
using AuthService.Persistence;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
	public interface ITokenService
	{
		string GenerateJwtToken(ApplicationUser user);
	}

	public class TokenService : ITokenService
	{
		private readonly JwtSettingsModel _jwtSettings;

		public TokenService(IOptions<JwtSettingsModel> jwtOptions)
		{
			_jwtSettings = jwtOptions.Value;
		}

		public string GenerateJwtToken(ApplicationUser user)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.Name, user.UserName ?? ""),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				//new Claim("licenseId", user.LicenseId.ToString())

			};

			var key = new SymmetricSecurityKey(Convert.FromBase64String(_jwtSettings.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
