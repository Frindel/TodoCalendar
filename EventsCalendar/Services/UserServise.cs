using EventsCalendar.Data.Entities;
using EventsCalendar.Data.Directories;

namespace EventsCalendar.Services;

public class UserServise : IUserServise
{
	private ITokenServise _tokenServise;
	private IDirectory<User> _usersDirectory;

	public UserServise(ITokenServise tokenServise, IDirectory<User> usersDirectory)
	{
		_tokenServise = tokenServise;
		_usersDirectory = usersDirectory;
	}

	public User Register()
	{
		string refreshToken = _tokenServise.GenerateRefreshToken();

		_usersDirectory.Add(new User()
		{
			RefreshToken = refreshToken
		});

		return _usersDirectory.Get(u => u.RefreshToken == refreshToken);
	}
}