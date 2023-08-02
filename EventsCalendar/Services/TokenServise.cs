using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EventsCalendar.Data.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EventsCalendar.Services;

public class TokenServise : ITokenServise
{
	private IConfiguration _configuration;

	public TokenServise(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string GenerateJWTToken(User user)
	{
		List<Claim> claims = new List<Claim>()
		{
			new(ClaimTypes.NameIdentifier, user.Id.ToString())
		};

		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
		var tokenValidityInMinutes = _configuration.GetSection("Jwt:TokenValidityInMinutes").Get<int>();

		return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"], // издатель
			audience: _configuration["Jwt:Audience"], // поставщик
			claims: claims,
			expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
			signingCredentials: new SigningCredentials(authSigningKey,
				SecurityAlgorithms.HmacSha256
			)));
	}

	public string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}
}