using EventsCalendar.Data.Entities;
using EventsCalendar.Data.Directories;

namespace EventsCalendar.Services;

public interface ITokenServise
{
	public string GenerateJWTToken(User user);
	public string GenerateRefreshToken();
}