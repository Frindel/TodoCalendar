namespace EventsCalendar.Data.Entities;

public class User : BaseEntity
{
	public string RefreshToken { get; set; } = null!;
}