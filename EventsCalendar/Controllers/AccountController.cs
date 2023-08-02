using EventsCalendar.Model.Account;
using EventsCalendar.Services;
using EventsCalendar.Data.Entities;
using EventsCalendar.Data.Directories;
using Microsoft.AspNetCore.Mvc;

namespace EventsCalendar.Controllers;

[Route("api/account")]
public class AccountController : Controller
{
	private IUserServise _userServise;
	private ITokenServise _tokenServise;
	private IDirectory<User> _usersDirectory;

	public AccountController(IUserServise userServise, ITokenServise tokenServise, IDirectory<User> usersDirectory)
	{
		_userServise = userServise;
		_tokenServise = tokenServise;
		_usersDirectory = usersDirectory;
	}

	[HttpGet("register")]
	public IActionResult Register()
	{
		User user = _userServise.Register();
		string jwtToken = _tokenServise.GenerateJWTToken(user);

		return Ok(new RegisterResponse()
		{
			RefreshToken = user.RefreshToken,
			AccessToken = jwtToken
		});
	}

	[HttpPost("refresh-token")]
	public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest();

		User user = _usersDirectory.Get(u => u.RefreshToken == request.RefreshToken);

		if (user == null)
			return BadRequest();

		user.RefreshToken = _tokenServise.GenerateRefreshToken();
		_usersDirectory.Edit(user);

		return Ok(new RefreshTokenResponse()
		{
			RefreshToken = user.RefreshToken,
			AccessToken = _tokenServise.GenerateJWTToken(user)
		});
	}
}