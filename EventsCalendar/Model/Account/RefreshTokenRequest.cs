using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Model.Account;

public class RefreshTokenRequest
{
	[Required]
	public string RefreshToken { get; set; }
}